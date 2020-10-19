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
        private readonly Dictionary<uint, ICharacterController> _controllerByPartyId = new Dictionary<uint, ICharacterController>(2);

        private GameState.GameState _previousGameState = null;
        private GameState.GameState _currentGameState = null;
        
        private uint? _pendingSelectedActionId = null;

        public IGameState CurrentGameState => _currentGameState;
        public uint? WinningPartyId { get; private set; }

        public ISignal<IGameState, IReadOnlyList<IStateChangeEvent>> GameStateUpdated { get; }
        public ISignal<uint> CombatComplete { get; }
        
        public CombatManager(IReadOnlyCollection<IParty> parties, ISignal<IGameState, IReadOnlyList<IStateChangeEvent>> gameStateUpdatedSignal, ISignal<uint> combatCompleteSignal)
        {
            GameStateUpdated = gameStateUpdatedSignal ?? new GameStateUpdated();
            CombatComplete = combatCompleteSignal ?? new CombatComplete();

            _currentGameState = new GameState.GameState(GameState.GameState.NextId, CombatState.Invalid, null, new List<IInitiativePair>(10));
            
            _parties.AddRange(parties);
            foreach (IParty party in parties)
            {
                _controllerByPartyId[party.Id] = party.Controller ?? new RandomCharacterController();
                foreach (IInitiativeActor actor in party.Actors)
                {
                    if (actor is ITargetableActor targetableActor)
                        _allTargets.Add(targetableActor);

                    _currentGameState.RawInitiativeOrder.Add(new InitiativePair(actor, (float) RANDOM.NextDouble() * 90f));
                }
            }

            SendOutgoingGameState();

            Task.Run(CombatThread);
        }

        private void CombatThread()
        {
            WinningPartyId = null;
            _currentGameState.State = CombatState.Active;
            
            uint winningParty = 0u;
            while (!GetWinningPartyId(out winningParty))
            {
                // Get Active Entity
                InitiativePair activeInitPair = GetNextEntity();
                IInitiativeActor activeEntity = activeInitPair.Entity;

                // Update GameState with ActiveEntity
                _previousGameState = (GameState.GameState) _currentGameState.Copy();
                _currentGameState.ActiveActor = activeEntity;
                _currentGameState.Id = GameState.GameState.NextId;
                SendOutgoingGameState();

                _previousGameState = (GameState.GameState) _currentGameState.Copy();
                _currentGameState.Id = GameState.GameState.NextId;

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
                }
                else
                {
                    activeInitPair.Initiative -= 100;
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
            List<IStateChangeEvent> changeEvents = new List<IStateChangeEvent>(10);

            if (_previousGameState != null)
            {
                changeEvents.AddRange(GetDiffGameStateChanges(_previousGameState, _currentGameState));
            }

            GameStateUpdated.Fire(_currentGameState.Copy(), changeEvents);
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

            foreach (IInitiativePair initPair in _currentGameState.RawInitiativeOrder)
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
            foreach (IInitiativePair tuple in _currentGameState.RawInitiativeOrder)
            {
                if (tuple is InitiativePair mutableTuple)
                {
                    mutableTuple.Initiative += tuple.Entity.GetInitiative();
                }
            }

            _currentGameState.RawInitiativeOrder.Sort((lhs, rhs) => rhs.Initiative.CompareTo(lhs.Initiative));
        }

        private InitiativePair GetNextEntity()
        {
            while (_currentGameState.InitiativeOrder[0].Initiative < 100f)
            {
                IncrementInitiative();
            }

            return _currentGameState.InitiativeOrder[0] as InitiativePair;
        }

        private void OnActionSelected(uint actionId)
        {
            _pendingSelectedActionId = actionId;
        }
    }
}