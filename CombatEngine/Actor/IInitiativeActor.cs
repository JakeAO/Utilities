using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Action;

namespace SadPumpkin.Util.CombatEngine.Actor
{
    /// <summary>
    /// Interface which defines the base Actor for combat.
    /// </summary>
    public interface IInitiativeActor : ICopyable<IInitiativeActor>
    {
        /// <summary>
        /// Unique Id used to track this object.
        /// </summary>
        uint Id { get; }

        /// <summary>
        /// Party Id of the party which this Actor belongs to.
        /// </summary>
        uint Party { get; }

        /// <summary>
        /// Name of this Actor.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Is this Actor currently an active combatant.
        /// </summary>
        /// <returns><c>True</c> if the Actor is alive, otherwise <c>False</c>.</returns>
        bool IsAlive();

        /// <summary>
        /// Get this Actor's initiative step.
        /// </summary>
        /// <returns>Initiative step of the Actor.</returns>
        float GetInitiative();

        /// <summary>
        /// Get all Actions which this Actor can perform on their turn.
        /// </summary>
        /// <param name="possibleTargets">All targetable Actors in combat.</param>
        /// <returns>Collection of all available Actions for this Actor.</returns>
        IReadOnlyCollection<IAction> GetAllActions(IReadOnlyCollection<ITargetableActor> possibleTargets);
    }
}