using System.Collections.Generic;
using System.Linq;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.GameState
{
    public class GameState : IGameState
    {
        private static uint _nextId = 150000;
        public static uint NextId => ++_nextId;

        public uint Id { get; set; }
        public CombatState State { get; set; }
        public IInitiativeActor ActiveActor { get; set; }
        public IReadOnlyList<IInitiativePair> InitiativeOrder { get; set; }

        public List<IInitiativePair> RawInitiativeOrder { get; set; }

        public GameState(
            uint id,
            CombatState state,
            IInitiativeActor activeActor,
            IReadOnlyList<IInitiativePair> initiative)
        {
            Id = id;
            State = state;
            ActiveActor = activeActor;
            if (initiative is List<IInitiativePair> asList)
            {
                InitiativeOrder = RawInitiativeOrder = asList;
            }
            else
            {
                InitiativeOrder = new List<IInitiativePair>(initiative);
                RawInitiativeOrder = null;
            }
        }

        public IGameState Copy()
        {
            return new GameState(
                Id,
                State,
                ActiveActor?.Copy(),
                InitiativeOrder.Select(x => x.Copy()).ToArray());
        }
    }
}