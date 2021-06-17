using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.Initiatives
{
    /// <summary>
    /// Interface defining an ordered collection of Actors based on their relative initiative values.
    /// </summary>
    public interface IInitiativeQueue
    {
        /// <summary>
        /// Get this Queue's initiative threshold.
        /// </summary>
        float InitiativeThreshold { get; }

        /// <summary>
        /// Get the current state of the initiative queue, in Actor-Initiative pairs.
        /// </summary>
        /// <returns>A collection of each actor and their current initiative.</returns>
        IEnumerable<(uint actorId, float initiative)> GetCurrentQueue();

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
        /// Manually change an Actor's current initiative.
        /// </summary>
        /// <param name="actorId">Actor to change initiative of.</param>
        /// <param name="changeValue">Amount to change Actor's current initiative by.</param>
        void Update(uint actorId, float changeValue);
        
        /// <summary>
        /// Remove an Actor from the initiative queue.
        /// </summary>
        /// <param name="actorId">Actor to remove from the queue.</param>
        /// <returns><c>True</c> if the Actor was removed, otherwise <c>False</c>.</returns>
        bool Remove(uint actorId);

        /// <summary>
        /// Generate a preview of the upcoming initiative order.
        /// </summary>
        /// <param name="previewLength">Number of initiative cycles to calculate.</param>
        /// <returns>Collection of Actor ids in the order of their upcoming turns.</returns>
        IEnumerable<uint> GetPreview(int previewLength);
    }
}