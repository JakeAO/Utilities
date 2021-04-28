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
using SadPumpkin.Util.CombatEngine.InitiativeSorter;
using SadPumpkin.Util.CombatEngine.Party;
using SadPumpkin.Util.CombatEngine.Signals;
using SadPumpkin.Util.CombatEngine.StateChangeEvents;
using SadPumpkin.Util.CombatEngine.StatMap;
using SadPumpkin.Util.CombatEngine.WinningPartyProvider;
using SadPumpkin.Util.Signals;

namespace SadPumpkin.Util.CombatEngine
{
    public class CombatManager
    {
        private const float INITIATIVE_THRESHOLD = 100f;
        
        private static readonly Random RANDOM = new Random();

        private readonly IWinningPartyProvider _winningPartyProvider;
        private readonly IInitiativeSorter _initiativeSorter;
        
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
        
        public CombatManager(
            IWinningPartyProvider winningPartyProvider,
            IInitiativeSorter initiativeSorter,
            IReadOnlyCollection<IParty> parties, 
            ISignal<IGameState, IReadOnlyList<IStateChangeEvent>> gameStateUpdatedSignal, 
            ISignal<uint> combatCompleteSignal)
        {
            _winningPartyProvider = winningPartyProvider ?? new OneAliveWinningPartyProvider();
            _initiativeSorter = initiativeSorter ?? new ThresholdInitiativeSorter(INITIATIVE_THRESHOLD);
            
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

                    _currentGameState.RawInitiativeOrder.Add(new InitiativePair(actor, (float) RANDOM.NextDouble() * INITIATIVE_THRESHOLD * 0.9f));
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
            while (!_winningPartyProvider.TryGetWinner(_parties, out winningParty))
            {
                // Get Active Entity
                IInitiativePair activeInitPair = _initiativeSorter.GetNext(_currentGameState.InitiativeOrder);
                IInitiativeActor activeEntity = activeInitPair.Entity;

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
                        do
                        {
                            Thread.Sleep(250);
                        } while (!_pendingSelectedActionId.HasValue);

                        // Validate response
                        actionsForEntity.TryGetValue(_pendingSelectedActionId.Value, out selectedAction);

                    } while (selectedAction == null || !selectedAction.Available);

                    // Apply effects of Action
                    initiativeCost = -selectedAction.Ability.Speed;

                    selectedAction.Ability.Cost.Pay(selectedAction.Source);
                    selectedAction.Ability.Effect.Apply(selectedAction.Source, selectedAction.Targets);
                }

                // Modify active actor's initiative
                if (activeInitPair is IWritableInitiativePair writableInitiativePair)
                {
                    writableInitiativePair.IncrementInitiative(initiativeCost);
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
            if (oldInitPair.Entity is IPlayerCharacterActor oldPlayerCharacter &&
                newInitPair.Entity is IPlayerCharacterActor newPlayerCharacter)
            {
                foreach (EquipmentSlot equipmentSlot in Enum.GetValues(typeof(EquipmentSlot)))
                {
                    if (oldPlayerCharacter.Equipment[equipmentSlot]?.Id != newPlayerCharacter.Equipment[equipmentSlot]?.Id)
                    {
                        yield return new CharacterEquipmentChangedEvent(
                            oldStateId, newStateId,
                            oldPlayerCharacter,
                            equipmentSlot, oldPlayerCharacter.Equipment[equipmentSlot], newPlayerCharacter.Equipment[equipmentSlot]);
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
                            oldCharacter,
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
                        oldActor,
                        oldActor.IsAlive(), newActor.IsAlive());
                }
            }
        }

        private void OnActionSelected(uint actionId)
        {
            _pendingSelectedActionId = actionId;
        }
    }
}