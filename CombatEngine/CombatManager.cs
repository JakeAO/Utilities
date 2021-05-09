using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.ActorChangeCalculator;
using SadPumpkin.Util.CombatEngine.CharacterControllers;
using SadPumpkin.Util.CombatEngine.Events;
using SadPumpkin.Util.CombatEngine.Initiatives;
using SadPumpkin.Util.CombatEngine.Party;
using SadPumpkin.Util.CombatEngine.WinningPartyCalculator;
using SadPumpkin.Util.Events;

namespace SadPumpkin.Util.CombatEngine
{
    /// <summary>
    /// Object which handles the execution of turn-based combat when provided with appropriate
    /// implementations of certain interfaces.
    /// </summary>
    public class CombatManager
    {
        private const int ACTOR_CHANGE_SLEEP_TIME = 1000;
        private const int ACTION_REQUEST_SLEEP_TIME = 250;

        private static readonly Random RANDOM = new Random();

        private readonly List<IParty> _parties = new List<IParty>(2);
        private readonly List<ITargetableActor> _allTargets = new List<ITargetableActor>(10);
        private readonly Dictionary<uint, ICharacterController> _controllerByPartyId = new Dictionary<uint, ICharacterController>(2);

        private readonly IStandardActionGenerator _standardActionGenerator;
        private readonly IWinningPartyCalculator _winningPartyCalculator;
        private readonly IActorChangeCalculator _actorChangeCalculator;
        private readonly IInitiativeQueue _initiativeQueue = null;
        private readonly IEventQueue _eventQueue = null;

        private CombatState _combatState = CombatState.Invalid;
        private uint? _pendingSelectedActionId = null;

        public IInitiativeQueue InitiativeQueue => _initiativeQueue;
        public IEventQueue EventData => _eventQueue;

        /// <summary>
        /// Construct a new CombatManager object with the provided data.
        /// </summary>
        /// <param name="parties">All participants in combat, grouped by their Party affiliation.</param>
        /// <param name="standardActionGenerator">Supplier of standard actions which all Actors have access to.</param>
        /// <param name="winningPartyCalculator">Calculator for how CombatManager decides when combat is finished and which party won.</param>
        /// <param name="actorChangeCalculator">Calculator for changes within an actor's properties.</param>
        /// <param name="initiativeQueue">Implementation of an initiative queue for use in this combat session.</param>
        /// <param name="eventQueue">Implementation of an event queue for use in this combat session.</param>
        public CombatManager(
            IReadOnlyCollection<IParty> parties,
            IStandardActionGenerator standardActionGenerator,
            IWinningPartyCalculator winningPartyCalculator,
            IActorChangeCalculator actorChangeCalculator,
            IInitiativeQueue initiativeQueue,
            IEventQueue eventQueue)
        {
            _standardActionGenerator = standardActionGenerator ?? new NullStandardActionGenerator();
            _winningPartyCalculator = winningPartyCalculator ?? new AnyAliveWinningPartyCalculator();
            _actorChangeCalculator = actorChangeCalculator ?? new NullActorChangeCalculator();
            _initiativeQueue = initiativeQueue ?? new InitiativeQueue(100f);
            _eventQueue = eventQueue ?? new EventQueue();

            _parties.AddRange(parties);
            foreach (IParty party in parties)
            {
                _controllerByPartyId[party.Id] = party.Controller ?? new RandomCharacterController();
                foreach (IInitiativeActor actor in party.Actors)
                {
                    if (actor is ITargetableActor targetableActor)
                        _allTargets.Add(targetableActor);

                    float startingInitiative = (float) RANDOM.NextDouble() * _initiativeQueue.InitiativeThreshold * 90f;
                    _initiativeQueue.Add(actor, startingInitiative);
                }
            }

            _combatState = CombatState.Init;
            _eventQueue.EnqueueEvent(new CombatStateChangedEvent(_combatState));
        }

        /// <summary>
        /// Begin executing the combat sequence.
        /// </summary>
        /// <param name="autoThread">Should the CombatManager create it's own thread to run the combat sequence on.</param>
        public void Start(bool autoThread)
        {
            if (autoThread)
            {
                Task.Run(CombatThread);
            }
            else
            {
                CombatThread();
            }
        }

        private void CombatThread()
        {
            _combatState = CombatState.Active;
            _eventQueue.EnqueueEvent(new CombatStateChangedEvent(_combatState));
            _eventQueue.EnqueueEvent(new CombatStartedEvent());

            uint winningParty = LoopUntilCombatComplete();

            _combatState = CombatState.Completed;
            _eventQueue.EnqueueEvent(new CombatStateChangedEvent(_combatState));
            _eventQueue.EnqueueEvent(new CombatCompletedEvent(winningParty));
        }

        private uint LoopUntilCombatComplete()
        {
            uint winningParty;
            while (!_winningPartyCalculator.GetWinningPartyId(_parties, out winningParty))
            {
                // Get Active Entity
                IInitiativeActor activeEntity = _initiativeQueue.GetNext();
                _eventQueue.EnqueueEvent(new ActiveActorChangedEvent(activeEntity.Id));

                // Prompt Action
                float initiativeReduction = -_initiativeQueue.InitiativeThreshold;
                if (activeEntity.IsAlive())
                {
                    // Get Controller and Entity Actions for Active Entity
                    Dictionary<uint, IAction> actionsForEntity = activeEntity
                        .GetAllActions(_allTargets)
                        .ToDictionary(x => x.Id);

                    // Add Standard Entity Actions
                    foreach (IAction standardAction in _standardActionGenerator.GetActions(activeEntity))
                    {
                        actionsForEntity[standardAction.Id] = standardAction;
                    }

                    // Send options and wait for valid response
                    IAction selectedAction = SendActionsAndWaitForResponse(activeEntity, _controllerByPartyId[activeEntity.Party], actionsForEntity);

                    // Set initiative cost
                    initiativeReduction = -selectedAction.Speed;

                    // Inform event queue we're applying an action
                    _eventQueue.EnqueueEvent(new ActorActionTakenEvent(selectedAction));

                    // Pay cost(s) for Action
                    _eventQueue.EnqueueEvents(ApplyActionCost(selectedAction, _actorChangeCalculator));

                    // Apply effect(s) for Action
                    _eventQueue.EnqueueEvents(ApplyActionEffect(selectedAction, _actorChangeCalculator));
                }

                // Update active entity's initiative value in the queue
                _initiativeQueue.Update(activeEntity, initiativeReduction);

                // Rest for a bit
                Thread.Sleep(ACTOR_CHANGE_SLEEP_TIME);
            }

            return winningParty;
        }

        private IAction SendActionsAndWaitForResponse(
            IInitiativeActor activeEntity,
            ICharacterController activeController,
            IReadOnlyDictionary<uint, IAction> availableActions)
        {
            IAction selectedAction;
            do
            {
                // Send Actions to Controller
                _pendingSelectedActionId = null;
                activeController.SelectAction(activeEntity, availableActions, OnActionSelected);

                // Wait for Controller response
                do
                {
                    Thread.Sleep(ACTION_REQUEST_SLEEP_TIME);
                } while (!_pendingSelectedActionId.HasValue);

                // Validate response
                availableActions.TryGetValue(_pendingSelectedActionId.Value, out selectedAction);

                _pendingSelectedActionId = null;

            } while (selectedAction == null || !selectedAction.Available);

            return selectedAction;
        }

        public void OnActionSelected(uint actionId)
        {
            _pendingSelectedActionId = actionId;
        }

        private static IEnumerable<ICombatEventData> ApplyActionCost(IAction selectedAction, IActorChangeCalculator actorChangeCalculator)
        {
            // Pre-apply copy
            IInitiativeActor sourceBeforeCost = selectedAction.Source.Copy();

            // Apply
            selectedAction.Cost.Pay(selectedAction.Source, selectedAction.ActionSource);

            // Post-apply comparison
            return actorChangeCalculator.GetChangeEvents(sourceBeforeCost, selectedAction.Source);
        }

        private static IEnumerable<ICombatEventData> ApplyActionEffect(IAction selectedAction, IActorChangeCalculator actorChangeCalculator)
        {
            // Pre-apply copies
            IInitiativeActor sourceBeforeEffect = selectedAction.Source.Copy();
            IEnumerable<IInitiativeActor> targetsBeforeEffect = selectedAction.Targets.Select(t => t.Copy());

            // Apply
            selectedAction.Effect.Apply(selectedAction.Source, selectedAction.Targets);

            // Post-apply comparison
            foreach (ICombatEventData eventData in actorChangeCalculator.GetChangeEvents(sourceBeforeEffect, selectedAction.Source))
            {
                yield return eventData;
            }

            foreach (var beforeAfterPair in targetsBeforeEffect.ToDictionary(
                x => x,
                x => selectedAction.Targets.First(y => y.Id == x.Id)))
            {
                foreach (ICombatEventData combatEventData in actorChangeCalculator.GetChangeEvents(beforeAfterPair.Key, beforeAfterPair.Value))
                {
                    yield return combatEventData;
                }
            }
        }
    }
}