using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.GameState
{
    /// <summary>
    /// Current snapshot of the combat sequence.
    /// </summary>
    public interface IGameState : ICopyable<IGameState>
    {
        /// <summary>
        /// Unique Id of the combat snapshot.
        /// </summary>
        uint Id { get; }

        /// <summary>
        /// Current mode of the combat sequence.
        /// </summary>
        CombatState State { get; }

        /// <summary>
        /// Actor currently taking Actions. Can be null.
        /// </summary>
        IInitiativeActor ActiveActor { get; }
        
        /// <summary>
        /// Preview of the upcoming turns ordered by initiative.
        /// </summary>
        IReadOnlyList<IInitiativeActor> InitiativePreview { get; }
    }
}