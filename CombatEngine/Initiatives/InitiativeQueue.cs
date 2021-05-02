using System;
using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.Initiatives
{
    /// <summary>
    /// Implementation of an initiative queue.
    /// </summary>
    public class InitiativeQueue : IInitiativeQueue
    {
        private class InitPair : IComparable<InitPair>
        {
            public IInitiativeActor Actor { get; }
            public float Initiative { get; set; }

            public InitPair(IInitiativeActor actor, float initiative)
            {
                Actor = actor;
                Initiative = initiative;
            }

            public InitPair((IInitiativeActor, float) data)
                : this(data.Item1, data.Item2)
            {
            }

            public InitPair(InitPair other)
                : this(other.Actor, other.Initiative)
            {

            }

            public int CompareTo(InitPair other)
            {
                int result = other.Initiative.CompareTo(Initiative);
                if (result != 0)
                    return result;

                return Actor.Id.CompareTo(other.Actor.Id);
            }
        }

        private static readonly Random RANDOM = new Random();

        private readonly float _targetValue = 100f;
        private readonly List<InitPair> _queue = new List<InitPair>();

        /// <summary>
        /// Construct a new, empty queue.
        /// </summary>
        /// <param name="targetValue">Limit which Actors' initiative must exceed to become active.</param>
        public InitiativeQueue(float targetValue)
        {
            _targetValue = targetValue;
        }

        /// <summary>
        /// Construct a new queue with the provided Actors, generating random starting initiatives.
        /// </summary>
        /// <param name="targetValue">Limit which Actors' initiative must exceed to become active.</param>
        /// <param name="actors">Collection of Actors to add to the queue.</param>
        public InitiativeQueue(float targetValue, IReadOnlyCollection<IInitiativeActor> actors)
            : this(targetValue)
        {
            foreach (var actor in actors)
            {
                _queue.Add(new InitPair(actor, (float) (RANDOM.NextDouble() * _targetValue)));
            }

            SortQueue(_queue);
        }

        /// <summary>
        /// Construct a new queue with the provided Actors and initiatives.
        /// </summary>
        /// <param name="targetValue">Limit which Actors' initiative must exceed to become active.</param>
        /// <param name="actors">Collection of Actors and their starting initiative values to add to the queue.</param>
        public InitiativeQueue(float targetValue, IReadOnlyCollection<(IInitiativeActor actor, float initialValue)> actors)
            : this(targetValue)
        {
            foreach (var actorTuple in actors)
            {
                _queue.Add(new InitPair(actorTuple));
            }

            SortQueue(_queue);
        }

        /// <summary>
        /// Get this Queue's initiative threshold.
        /// </summary>
        public float InitiativeThreshold => _targetValue;

        /// <summary>
        /// Get the next Actor in initiative queue.
        /// </summary>
        /// <returns>The next Actor, or null if the collection is empty.</returns>
        public IInitiativeActor GetNext()
        {
            if (_queue.Count == 0)
                return null;

            SortQueue(_queue);
            if (_queue[0].Initiative < _targetValue)
                IncrementQueue(_queue, _targetValue);

            return _queue[0].Actor;
        }

        /// <summary>
        /// Add a new Actor to the initiative queue.
        /// </summary>
        /// <param name="actor">Actor to add to the queue.</param>
        /// <param name="initialValue">Actor's current initiative value.</param>
        /// <returns><c>True</c> if the Actor was added, otherwise <c>False</c>.</returns>
        public bool Add(IInitiativeActor actor, float initialValue)
        {
            for (int i = 0; i < _queue.Count; i++)
            {
                if (_queue[i].Actor.Equals(actor))
                    return false;
            }

            _queue.Add(new InitPair(actor, initialValue));
            return true;
        }

        /// <summary>
        /// Remove an Actor from the initiative queue.
        /// </summary>
        /// <param name="actor">Actor to remove from the queue.</param>
        /// <returns><c>True</c> if the Actor was removed, otherwise <c>False</c>.</returns>
        public bool Remove(IInitiativeActor actor)
        {
            for (int i = 0; i < _queue.Count; i++)
            {
                if (_queue[i].Actor.Equals(actor))
                {
                    _queue.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Update an Actor's initiative value.
        /// </summary>
        /// <param name="actor">Actor to update the initiative of.</param>
        /// <param name="shift">Amount by which to update the Actor's initiative.</param>
        /// <returns><c>True</c> if the Actor's initiative was updated, otherwise <c>False</c>.</returns>
        public bool Update(IInitiativeActor actor, float shift)
        {
            for (int i = 0; i < _queue.Count; i++)
            {
                if (_queue[i].Actor.Equals(actor))
                {
                    _queue[i].Initiative += shift;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Generate a preview of the upcoming initiative order.
        /// </summary>
        /// <param name="previewLength">Number of initiative cycles to calculate.</param>
        /// <returns>Collection of Actors in the order of their upcoming turns.</returns>
        public IReadOnlyList<IInitiativeActor> GetPreview(int previewLength)
        {
            List<IInitiativeActor> result = new List<IInitiativeActor>(previewLength);

            if (_queue.Count > 0)
            {
                List<InitPair> tempQueue = _queue.ConvertAll(x => new InitPair(x));
                for (int i = 0; i < previewLength; i++)
                {
                    SortQueue(tempQueue);
                    while (tempQueue[0].Initiative < _targetValue)
                        IncrementQueue(tempQueue, _targetValue);
                    result.Add(tempQueue[0].Actor);
                    tempQueue[0].Initiative -= _targetValue;
                }
            }

            return result;
        }

        /// <summary>
        /// Increment the provided queue until an active Actor can be determined.
        /// </summary>
        /// <param name="queue">Queue to update.</param>
        /// <param name="targetValue">Target initiative value which Actors must meet.</param>
        private static void IncrementQueue(List<InitPair> queue, float targetValue)
        {
            if (queue.Count == 0)
                return;

            bool incrementRequired = true;

            // Determine if we need to go through the increment process at all
            for (int i = 0; i < queue.Count; i++)
            {
                if (queue[i].Initiative >= targetValue)
                {
                    incrementRequired = false;
                    break;
                }
            }

            // Continue incrementing the queue until we have at least one initiative above the target
            while (incrementRequired)
            {
                for (int i = 0; i < queue.Count; i++)
                {
                    queue[i].Initiative += Math.Max(1, queue[i].Actor.GetInitiative());

                    if (queue[i].Initiative >= targetValue)
                        incrementRequired = false;
                }
            }

            SortQueue(queue);
        }

        /// <summary>
        /// Sort the provided queue using the InitPair's default comparer.
        /// </summary>
        /// <param name="queue">Queue to sort.</param>
        private static void SortQueue(List<InitPair> queue)
        {
            queue.Sort();
        }
    }
}