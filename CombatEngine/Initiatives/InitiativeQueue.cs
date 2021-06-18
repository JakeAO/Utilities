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

        private readonly uint _targetValue = 100;
        private readonly List<InitPair> _queue = new List<InitPair>();

        /// <summary>
        /// Construct a new, empty queue.
        /// </summary>
        /// <param name="targetValue">Limit which Actors' initiative must exceed to become active.</param>
        public InitiativeQueue(uint targetValue)
        {
            _targetValue = targetValue;
        }

        /// <summary>
        /// Construct a new queue with the provided Actors, generating random starting initiatives.
        /// </summary>
        /// <param name="targetValue">Limit which Actors' initiative must exceed to become active.</param>
        /// <param name="actors">Collection of Actors to add to the queue.</param>
        public InitiativeQueue(uint targetValue, IEnumerable<IInitiativeActor> actors)
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
        public InitiativeQueue(uint targetValue, IEnumerable<(IInitiativeActor actor, float initialValue)> actors)
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
        public uint InitiativeThreshold => _targetValue;

        /// <summary>
        /// Get the current state of the initiative queue, in Actor-Initiative pairs.
        /// </summary>
        /// <returns>A collection of each actor and their current initiative.</returns>
        public IEnumerable<(uint actorId, float initiative)> GetCurrentQueue()
        {
            foreach (InitPair initPair in _queue)
            {
                yield return (initPair.Actor.Id, initPair.Initiative);
            }
        }

        /// <summary>
        /// Get the next Actor in initiative queue.
        /// </summary>
        /// <returns>The next Actor, or null if the collection is empty.</returns>
        public IInitiativeActor GetNext()
        {
            if (_queue.Count == 0)
                return null;

            SortQueue(_queue);
            while (_queue[0].Initiative < _targetValue)
                IncrementQueue(_queue);

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
        /// Manually change an Actor's current initiative.
        /// </summary>
        /// <param name="actorId">Actor to change initiative of.</param>
        /// <param name="changeValue">Amount to change Actor's current initiative by.</param>
        public void Update(uint actorId, uint changeValue)
        {
            for (int i = 0; i < _queue.Count; i++)
            {
                if (_queue[i].Actor.Id == actorId)
                {
                    _queue[i].Initiative -= changeValue;
                    return;
                }
            }
        }

        /// <summary>
        /// Remove an Actor from the initiative queue.
        /// </summary>
        /// <param name="actorId">Actor to remove from the queue.</param>
        /// <returns><c>True</c> if the Actor was removed, otherwise <c>False</c>.</returns>
        public bool Remove(uint actorId)
        {
            for (int i = 0; i < _queue.Count; i++)
            {
                if (_queue[i].Actor.Id == actorId)
                {
                    _queue.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Generate a preview of the upcoming initiative order.
        /// </summary>
        /// <param name="previewLength">Number of initiative cycles to calculate.</param>
        /// <returns>Collection of Actor ids in the order of their upcoming turns.</returns>
        public IEnumerable<uint> GetPreview(int previewLength)
        {
            if (_queue.Count > 0)
            {
                List<InitPair> tempQueue = _queue.ConvertAll(x => new InitPair(x));
                for (int i = 0; i < previewLength; i++)
                {
                    SortQueue(tempQueue);
                    while (tempQueue[0].Initiative < _targetValue)
                        IncrementQueue(tempQueue);

                    yield return tempQueue[0].Actor.Id;

                    tempQueue[0].Initiative -= _targetValue;
                }
            }
        }

        /// <summary>
        /// Increment the provided queue until an active Actor can be determined.
        /// </summary>
        /// <param name="queue">Queue to update.</param>
        private static void IncrementQueue(List<InitPair> queue)
        {
            if (queue.Count == 0)
                return;

            for (int i = 0; i < queue.Count; i++)
            {
                queue[i].Initiative += Math.Max(1, queue[i].Actor.GetInitiative());
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