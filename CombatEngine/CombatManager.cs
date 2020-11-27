using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.CharacterControllers;
using SadPumpkin.Util.CombatEngine.GameState;
using SadPumpkin.Util.CombatEngine.Party;
using SadPumpkin.Util.CombatEngine.Signals;
using SadPumpkin.Util.Signals;

namespace SadPumpkin.Util.CombatEngine
{
    public class CombatManager
    {
        private static readonly Random RANDOM = new Random();

        private readonly List<IParty> _parties = new List<IParty>(2);
        private readonly List<ITargetableActor> _allTargets = new List<ITargetableActor>(10);
        private readonly Dictionary<uint, ICharacterController> _controllerByPartyId = new Dictionary<uint, ICharacterController>(2);
        private readonly IStandardActionGenerator _standardActionGenerator;
        
        private GameState.GameState _previousGameState = null;
        private GameState.GameState _currentGameState = null;
        
        private uint? _pendingSelectedActionId = null;

        public IGameState CurrentGameState => _currentGameState;
        public uint? WinningPartyId { get; private set; }

        public ISignal<IGameState> GameStateUpdated { get; }
        public ISignal<uint> CombatComplete { get; }
        
        public CombatManager(IReadOnlyCollection<IParty> parties, IStandardActionGenerator standardActionGenerator, ISignal<IGameState> gameStateUpdatedSignal, ISignal<uint> combatCompleteSignal)
        {
            _standardActionGenerator = standardActionGenerator ?? new NullStandardActionGenerator();
            
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
                    activeInitPair.Initiative -= selectedAction.Speed;

                    selectedAction.Cost.Pay(selectedAction.Source, selectedAction.ActionSource);
                    selectedAction.Effect.Apply(selectedAction.Source, selectedAction.Targets);
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
            GameStateUpdated.Fire(_currentGameState.Copy());
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