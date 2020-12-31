using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.Action
{
    /// <summary>
    /// Implementation of a standard action generator that returns no Actions.
    /// </summary>
    public class NullStandardActionGenerator : IStandardActionGenerator
    {
        /// <summary>
        /// Get a collection of standard Actions for the supplied Actor.
        /// </summary>
        /// <param name="actor">Actor to create standard Actions for.</param>
        /// <returns>Collection of standard Actions.</returns>
        public IEnumerable<IAction> GetActions(IInitiativeActor actor)
        {
            yield break;
        }
    }
}