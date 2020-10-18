using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.CharacterControllers;
using SadPumpkin.Util.CombatEngine.EquipMap;
using SadPumpkin.Util.CombatEngine.GameState;
using SadPumpkin.Util.CombatEngine.Party;
using SadPumpkin.Util.CombatEngine.Signals;
using SadPumpkin.Util.CombatEngine.StateChangeEvents;
using SadPumpkin.Util.CombatEngine.StatMap;
using SadPumpkin.Util.Signals;

namespace SadPumpkin.Util.CombatEngine
{
    public class CombatManager
    {
        private static readonly Random RANDOM = new Random();

        private readonly List<IParty> _parties = new List<IParty>(2);
        private readonly List<ITargetableActor> _allTargets = new List<ITargetableActor>(10);
        private readonly List<InitiativePair> _actorInitiativeList = new List<InitiativePair>(10);
        private readonly Dictionary<uint, ICharacterController> _controllerByPartyId = new Dictionary<uint, ICharacterController>(2);

        private GameState.GameState _currentGameState = null;
        private CombatState _state = CombatState.Invalid;
        private uint? _pendingSelectedActionId = null;
        
        public IGameState CurrentGameState => _currentGameState;
        public uint? WinningPartyId { get; private set; }

        public ISignal<IGameState, IReadOnlyList<IStateChangeEvent>> GameStateUpdated { get; }
        public ISignal<uint> CombatComplete { get; }
        
        public CombatManager(IReadOnlyCollection<IParty> parties, ISignal<IGameState, IReadOnlyList<IStateChangeEvent>> gameStateUpdatedSignal, ISignal<uint> combatCompleteSignal)
        {
            GameStateUpdated = gameStateUpdatedSignal ?? new GameStateUpdated();
            CombatComplete = combatCompleteSignal ?? new CombatComplete();
            
            _parties.AddRange(parties);
            foreach (IParty party in parties)
            {
                _controllerByPartyId[party.Id] = party.Controller ?? new RandomCharacterController();
                foreach (IInitiativeActor actor in party.Actors)
                {
                    if (actor is ITargetableActor targetableActor)
                        _allTargets.Add(targetableActor);

                    _actorInitiativeList.Add(new InitiativePair(actor, (float) RANDOM.NextDouble() * 90f));
                }
            }

            _state = CombatState.Invalid;
            UpdateCurrentGameState(null);

            Task.Run(CombatThread);
        }

        private void CombatThread()
        {
            IReadOnlyList<IStateChangeEvent> stateChangeEvents = null;

            WinningPartyId = null;
            _state = CombatState.Active;

            uint winningParty = 0u;
            while (!GetWinningPartyId(out winningParty))
            {
                // Get Active Entity
                InitiativePair activeInitPair = GetNextEntity();
                IInitiativeActor activeEntity = activeInitPair.Entity;

                // Update GameState with ActiveEntity
                stateChangeEvents = UpdateCurrentGameState(activeEntity);
                GameStateUpdated.Fire(_currentGameState, stateChangeEvents);

                // Prompt Action
                if (activeEntity.IsAlive())
                {
                    ICharacterController controller = _controllerByPartyId[activeEntity.Party];
                    Dictionary<uint, IAction> actionsForEntity = activeEntity
                        .GetAllActions(_allTargets)
                        .ToDictionary(x => x.Id);

                    // Wait for valid response
                    IAction selectedAction;
                    do
                    {
                        // Send Actions to Controller
                        _pendingSelectedActionId = null;
                        controller.SelectAction(activeEntity, actionsForEntity, OnActionSelected);

                        // Wait for Controller response
                        while (!_pendingSelectedActionId.HasValue)
                        {
                            Thread.Sleep(1000);
                        }

                        // Validate response
                        actionsForEntity.TryGetValue(_pendingSelectedActionId.Value, out selectedAction);

                    } while (selectedAction == null || !selectedAction.Available);

                    // Apply effects of Action
                    activeInitPair.Initiative -= selectedAction.Ability.Speed;

                    selectedAction.Ability.Cost.Pay(selectedAction.Source);
                    selectedAction.Ability.Effect.Apply(selectedAction.Source, selectedAction.Targets);

                    // Update GameState with Effects of Action
                    stateChangeEvents = UpdateCurrentGameState(null);
                    GameStateUpdated.Fire(_currentGameState, stateChangeEvents);
                }
                else
                {
                    activeInitPair.Initiative -= 100;
                }

                // Rest for a bit
                Thread.Sleep(1000);
            }

            _state = CombatState.Completed;
            WinningPartyId = winningParty;
            
            stateChangeEvents = UpdateCurrentGameState(null);
            GameStateUpdated.Fire(_currentGameState, stateChangeEvents);
            
            CombatComplete.Fire(winningParty);
        }

        private IReadOnlyList<IStateChangeEvent> UpdateCurrentGameState(IInitiativeActor activeEntity)
        {
            IGameState previousGameState = _currentGameState;
            _currentGameState = new GameState.GameState(_state, activeEntity, _actorInitiativeList);

            return GetDiffGameStateChanges(previousGameState, _currentGameState);
        }

        private IReadOnlyList<IStateChangeEvent> GetDiffGameStateChanges(IGameState oldState, IGameState newState)
        {
            List<IStateChangeEvent> stateChangeEvents = new List<IStateChangeEvent>(1);

            // State
            if (oldState.State != newState.State)
            {
                stateChangeEvents.Add(new CombatStateChangedEvent(oldState.Id, newState.Id, oldState.State, newState.State));
            }

            // ActiveEntity
            if (oldState.ActiveActor?.Id != newState.ActiveActor?.Id)
            {
                stateChangeEvents.Add(new ActiveActorChangedEvent(oldState.Id, newState.Id, oldState.ActiveActor?.Id, newState.ActiveActor?.Id));
            }

            // InitiativeActors
            foreach (var newPair in newState.InitiativeOrder)
            {
                var oldPair = oldState.InitiativeOrder.First(x => x.Entity.Id == newPair.Entity.Id);

                stateChangeEvents.AddRange(GetDiffInitiativePairChanges(oldState.Id, newState.Id, oldPair, newPair));
            }

            return stateChangeEvents;
        }

        private IEnumerable<IStateChangeEvent> GetDiffInitiativePairChanges(
            uint oldStateId, uint newStateId,
            IInitiativePair oldInitPair, IInitiativePair newInitPair)
        {
            if (Math.Abs(oldInitPair.Initiative - newInitPair.Initiative) > float.Epsilon)
            {
                yield return new ActorInitiativeChangedEvent(
                    oldStateId, newStateId,
                    oldInitPair.Entity.Id,
                    oldInitPair.Initiative, newInitPair.Initiative);
            }

            if (oldInitPair.Entity is IPlayerCharacterActor oldPlayerCharacter &&
                newInitPair.Entity is IPlayerCharacterActor newPlayerCharacter)
            {
                foreach (EquipmentSlot equipmentSlot in Enum.GetValues(typeof(EquipmentSlot)))
                {
                    if (oldPlayerCharacter.Equipment[equipmentSlot]?.Id != newPlayerCharacter.Equipment[equipmentSlot]?.Id)
                    {
                        yield return new CharacterEquipmentChangedEvent(
                            oldStateId, newStateId,
                            oldPlayerCharacter.Id,
                            equipmentSlot, oldPlayerCharacter.Equipment[equipmentSlot]?.Id, newPlayerCharacter.Equipment[equipmentSlot]?.Id);
                    }
                }
            }

            if (oldInitPair.Entity is ICharacterActor oldCharacter &&
                newInitPair.Entity is ICharacterActor newCharacter)
            {
                foreach (StatType statType in Enum.GetValues(typeof(StatType)))
                {
                    if (oldCharacter.Stats[statType] != newCharacter.Stats[statType])
                    {
                        yield return new CharacterStatChangedEvent(
                            oldStateId, newStateId,
                            oldCharacter.Id,
                            statType, oldCharacter.Stats[statType], newCharacter.Stats[statType]);
                    }
                }
            }

            if (oldInitPair.Entity is IInitiativeActor oldActor &&
                newInitPair.Entity is IInitiativeActor newActor)
            {
                if (oldActor.IsAlive() != newActor.IsAlive())
                {
                    yield return new ActorAlivenessChangedEvent(
                        oldStateId, newStateId,
                        oldActor.Id,
                        oldActor.IsAlive(), newActor.IsAlive());
                }
            }
        }

        private bool GetWinningPartyId(out uint winningParty)
        {
            winningParty = 0u;

            foreach (InitiativePair initPair in _actorInitiativeList)
            {
                IInitiativeActor actor = initPair.Entity;
                if (actor == null)
                    continue;
                if (!actor.IsAlive())
                    continue;

                if (winningParty == 0)
                {
                    winningParty = actor.Party;
                }
                else if (winningParty != actor.Party)
                {
                    return false;
                }
            }

            return winningParty != 0u;
        }

        private void IncrementInitiative()
        {
            foreach (InitiativePair tuple in _actorInitiativeList)
            {
                tuple.Initiative += tuple.Entity.GetInitiative();
            }

            _actorInitiativeList.Sort((lhs, rhs) => rhs.Initiative.CompareTo(lhs.Initiative));
        }

        private InitiativePair GetNextEntity()
        {
            while (_actorInitiativeList[0].Initiative < 100f)
            {
                IncrementInitiative();
            }

            return _actorInitiativeList[0];
        }

        private void OnActionSelected(uint actionId)
        {
            _pendingSelectedActionId = actionId;
        }
    }
}