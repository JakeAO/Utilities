using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.CharacterControllers;

namespace SadPumpkin.Util.CombatEngine.Party
{
    /// <summary>
    /// Interface defining a collection of Actors in combat.
    /// </summary>
    public interface IParty : IIdTracked
    {
        /// <summary>
        /// Character controller which handles Actions for this Party.
        /// </summary>
        ICharacterController Controller { get; }

        /// <summary>
        /// Collection of Actors which belong to this Party.
        /// </summary>
        IReadOnlyCollection<IInitiativeActor> Actors { get; }
    }
}