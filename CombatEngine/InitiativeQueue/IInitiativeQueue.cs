using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.InitiativeQueue
{
    /// <summary>
    /// Interface defining an ordered collection of Actors based on their relative initiative values.
    /// </summary>
    public interface IInitiativeQueue
    {
        /// <summary>
        /// Get the next Actor in initiative queue.
        /// </summary>
        /// <returns>The next Actor, or null if the collection is empty.</returns>
        IInitiativeActor GetNext();

        /// <summary>
        /// Add a new Actor to the initiative queue.
        /// </summary>
        /// <param name="actor">Actor to add to the queue.</param>
        /// <param name="initialValue">Actor's current initiative value.</param>
        /// <returns><c>True</c> if the Actor was added, otherwise <c>False</c>.</returns>
        bool Add(IInitiativeActor actor, float initialValue);

        /// <summary>
        /// Remove an Actor from the initiative queue.
        /// </summary>
        /// <param name="actor">Actor to remove from the queue.</param>
        /// <returns><c>True</c> if the Actor was removed, otherwise <c>False</c>.</returns>
        bool Remove(IInitiativeActor actor);

        /// <summary>
        /// Update an Actor's initiative value.
        /// </summary>
        /// <param name="actor">Actor to update the initiative of.</param>
        /// <param name="shift">Amount by which to update the Actor's initiative.</param>
        /// <returns><c>True</c> if the Actor's initiative was updated, otherwise <c>False</c>.</returns>
        bool Update(IInitiativeActor actor, float shift);

        /// <summary>
        /// Generate a preview of the upcoming initiative order.
        /// </summary>
        /// <param name="previewLength">Number of initiative cycles to calculate.</param>
        /// <returns>Collection of Actors in the order of their upcoming turns.</returns>
        IReadOnlyList<IInitiativeActor> GetPreview(int previewLength);
    }
}