using System.Collections.Generic;
using System.Linq;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.GameState
{
    public class GameState : IGameState
    {
        private static uint _nextId = 150000;
        public static uint NextId => ++_nextId;

        /// <summary>
        /// Unique Id of the combat snapshot.
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// Current mode of the combat sequence.
        /// </summary>
        public CombatState State { get; set; }

        /// <summary>
        /// Actor currently taking Actions. Can be null.
        /// </summary>
        public IInitiativeActor ActiveActor { get; set; }

        /// <summary>
        /// Preview of the upcoming turns ordered by initiative.
        /// </summary>
        public IReadOnlyList<IInitiativeActor> InitiativePreview { get; set; }

        /// <summary>
        /// Construct a new GameState with the provided values.
        /// </summary>
        /// <param name="id">Unique Id of the snapshot.</param>
        /// <param name="state">Current mode of the combat sequence.</param>
        /// <param name="activeActor">Currently active Actor selecting Actions.</param>
        /// <param name="initiativePreview">Preview of upcoming Actor turns.</param>
        public GameState(
            uint id,
            CombatState state,
            IInitiativeActor activeActor,
            IReadOnlyList<IInitiativeActor> initiativePreview)
        {
            Id = id;
            State = state;
            ActiveActor = activeActor;
            InitiativePreview = initiativePreview;
        }

        /// <summary>
        /// Create a deep copy of the current object.
        /// </summary>
        /// <returns>Duplicate of the current object.</returns>
        public IGameState Copy()
        {
            return new GameState(
                Id,
                State,
                ActiveActor?.Copy(),
                InitiativePreview.Select(x => x.Copy()).ToArray());
        }
    }
}