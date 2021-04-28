using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.CharacterControllers;
using SadPumpkin.Util.CombatEngine.GameState;
using SadPumpkin.Util.CombatEngine.InitiativeQueue;
using SadPumpkin.Util.CombatEngine.Party;
using SadPumpkin.Util.CombatEngine.Signals;
using SadPumpkin.Util.Signals;

namespace SadPumpkin.Util.CombatEngine
{
    /// <summary>
    /// Object which handles the execution of turn-based combat when provided with appropriate
    /// implementations of certain interfaces.
    /// </summary>
    public class CombatManager
    {
        private static readonly Random RANDOM = new Random();

        private readonly float _targetInitiative = 100f;
        private readonly List<IParty> _parties = new List<IParty>(2);
        private readonly List<ITargetableActor> _allTargets = new List<ITargetableActor>(10);
        private readonly Dictionary<uint, ICharacterController> _controllerByPartyId = new Dictionary<uint, ICharacterController>(2);
        private readonly IStandardActionGenerator _standardActionGenerator;
        private readonly IInitiativeQueue _initiativeQueue = null;

        private GameState.GameState _previousGameState = null;
        private GameState.GameState _currentGameState = null;

        private uint? _pendingSelectedActionId = null;

        public IGameState CurrentGameState => _currentGameState;
        public uint? WinningPartyId { get; private set; }

        public ISignal<IGameState> GameStateUpdated { get; }
        public ISignal<uint> CombatComplete { get; }

        /// <summary>
        /// Construct a new CombatManager object with the provided data.
        /// </summary>
        /// <param name="parties">All participants in combat, grouped by their Party affiliation.</param>
        /// <param name="standardActionGenerator">Supplier of standard actions which all Actors have access to.</param>
        /// <param name="targetInitiative">Initiative value which an Actor much reach in order to become the active Actor.</param>
        /// <param name="gameStateUpdatedSignal">Signal fired when the CombatManager updates the current GameState.</param>
        /// <param name="combatCompleteSignal">Signal fired when combat is complete (only one Party has living Actors).</param>
        public CombatManager(
            IReadOnlyCollection<IParty> parties,
            IStandardActionGenerator standardActionGenerator,
            float targetInitiative,
            ISignal<IGameState> gameStateUpdatedSignal,
            ISignal<uint> combatCompleteSignal)
        {
            _targetInitiative = targetInitiative;
            _initiativeQueue = new InitiativeQueue.InitiativeQueue(_targetInitiative);

            _standardActionGenerator = standardActionGenerator ?? new NullStandardActionGenerator();

            GameStateUpdated = gameStateUpdatedSignal ?? new GameStateUpdated();
            CombatComplete = combatCompleteSignal ?? new CombatComplete();

            _currentGameState = new GameState.GameState(GameState.GameState.NextId, CombatState.Invalid, null, new List<IInitiativeActor>(0));

            _parties.AddRange(parties);
            foreach (IParty party in parties)
            {
                _controllerByPartyId[party.Id] = party.Controller ?? new RandomCharacterController();
                foreach (IInitiativeActor actor in party.Actors)
                {
                    if (actor is ITargetableActor targetableActor)
                        _allTargets.Add(targetableActor);

                    _initiativeQueue.Add(actor, (float) (RANDOM.NextDouble() * 90));
                }
            }

            SendOutgoingGameState();
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
            WinningPartyId = null;
            _currentGameState.State = CombatState.Active;

            uint winningParty = 0u;
            while (!GetWinningPartyId(out winningParty))
            {
                // Get Active Entity
                IInitiativeActor activeEntity = _initiativeQueue.GetNext();

                // Update GameState with ActiveEntity
                _previousGameState = (GameState.GameState) _currentGameState.Copy();
                _currentGameState.ActiveActor = activeEntity;
                _currentGameState.Id = GameState.GameState.NextId;
                SendOutgoingGameState();

                _previousGameState = (GameState.GameState) _currentGameState.Copy();
                _currentGameState.Id = GameState.GameState.NextId;

                // Prompt Action
                float initiativeCost = -100f;
                if (activeEntity.IsAlive())
                {
                    // Get Controller and Entity Actions for Active Entity
                    ICharacterController controller = _controllerByPartyId[activeEntity.Party];
                    Dictionary<uint, IAction> actionsForEntity = activeEntity
                        .GetAllActions(_allTargets)
                        .ToDictionary(x => x.Id);

                    // Add Standard Entity Actions
                    foreach (IAction standardAction in _standardActionGenerator.GetActions(activeEntity))
                    {
                        actionsForEntity[standardAction.Id] = standardAction;
                    }

                    // Wait for valid response
                    IAction selectedAction;
                    do
                    {
                        // Send Actions to Controller
                        _pendingSelectedActionId = null;
                        controller.SelectAction(activeEntity, actionsForEntity, OnActionSelected);

                        // Wait for Controller response
                        do
                        {
                            Thread.Sleep(250);
                        } while (!_pendingSelectedActionId.HasValue);

                        // Validate response
                        actionsForEntity.TryGetValue(_pendingSelectedActionId.Value, out selectedAction);

                    } while (selectedAction == null || !selectedAction.Available);

                    // Apply effects of Action
                    _initiativeQueue.Update(activeEntity, -selectedAction.Speed);

                    selectedAction.Cost.Pay(selectedAction.Source, selectedAction.ActionSource);
                    selectedAction.Effect.Apply(selectedAction.Source, selectedAction.Targets);
                }

                // Update GameState with Effects of Action
                _currentGameState.ActiveActor = null;
                SendOutgoingGameState();

                // Rest for a bit
                Thread.Sleep(1000);
            }

            _previousGameState = (GameState.GameState) _currentGameState.Copy();
            _currentGameState.Id = GameState.GameState.NextId;
            _currentGameState.State = CombatState.Completed;

            WinningPartyId = winningParty;
            SendOutgoingGameState();

            CombatComplete.Fire(winningParty);
        }

        private void SendOutgoingGameState()
        {
            var copiedGameState = new GameState.GameState(
                _currentGameState.Id,
                _currentGameState.State,
                _currentGameState.ActiveActor?.Copy(),
                _initiativeQueue.GetPreview(5));
            GameStateUpdated.Fire(copiedGameState);
        }

        private bool GetWinningPartyId(out uint winningParty)
        {
            winningParty = 0u;

            foreach (IParty party in _parties)
            {
                foreach (IInitiativeActor actor in party.Actors)
                {
                    if (actor == null)
                        continue;
                    if (!actor.IsAlive())
                        continue;

                    if (winningParty == 0)
                    {
                        winningParty = actor.Party;
                        break;
                    }

                    if (winningParty != actor.Party)
                    {
                        return false;
                    }
                }
            }

            return winningParty != 0u;
        }

        private void OnActionSelected(uint actionId)
        {
            _pendingSelectedActionId = actionId;
        }
    }
}