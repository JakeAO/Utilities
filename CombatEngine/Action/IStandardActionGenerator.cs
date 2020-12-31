using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.Action
{
    /// <summary>
    /// Object which supplies standard actions for all Actors in combat.
    /// </summary>
    public interface IStandardActionGenerator
    {
        /// <summary>
        /// Get a collection of standard Actions for the supplied Actor.
        /// </summary>
        /// <param name="actor">Actor to create standard Actions for.</param>
        /// <returns>Collection of standard Actions.</returns>
        IEnumerable<IAction> GetActions(IInitiativeActor actor);
    }
}