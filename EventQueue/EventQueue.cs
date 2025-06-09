using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace SadPumpkin.Util.Events
{
    /// <summary>
    /// A thread-safe implementation of <see cref="IEventQueue"/> that manages an ordered collection of events.
    /// This queue is designed to facilitate playback of events in the UI and other systems.
    /// </summary>
    public class EventQueue : IEventQueue
    {
        /// <summary>
        /// The underlying thread-safe queue that stores event data.
        /// </summary>
        private readonly ConcurrentQueue<IEventData> _eventQueue = new ConcurrentQueue<IEventData>();

        /// <summary>
        /// Event fired when a new event is added to the queue.
        /// </summary>
        public event Action<IEventData> EventEnqueued;
        
        /// <summary>
        /// Event fired when an event is removed from the queue.
        /// </summary>
        public event Action<IEventData> EventDequeued;

        /// <summary>
        /// Gets the number of events currently in the queue.
        /// </summary>
        public int Count => _eventQueue.Count;

        /// <summary>
        /// Adds a single event to the queue if it is not null.
        /// </summary>
        /// <param name="eventData">The event data to be enqueued.</param>
        public void EnqueueEvent(IEventData eventData)
        {
            if (eventData != null)
            {
                _eventQueue.Enqueue(eventData);

                // Safely invoke the event handler
                try
                {
                    EventEnqueued?.Invoke(eventData);
                }
                catch (Exception ex)
                {
                    // Log the exception but don't let it disrupt the queue operation
                    Debug.WriteLine($"Exception in EventEnqueued handler: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Adds multiple events to the queue, ignoring any null events.
        /// </summary>
        /// <param name="eventDatas">Collection of event data to be enqueued.</param>
        public void EnqueueEvents(IEnumerable<IEventData> eventDatas)
        {
            if (eventDatas == null)
            {
                return;
            }
            
            foreach (IEventData eventData in eventDatas)
            {
                EnqueueEvent(eventData);
            }
        }

        /// <summary>
        /// Attempts to remove and return the oldest event from the queue.
        /// </summary>
        /// <param name="eventData">When this method returns, contains the dequeued event data, if available; otherwise, the default value.</param>
        /// <returns>True if an event was successfully dequeued; otherwise, false.</returns>
        public bool TryDequeueEvent(out IEventData eventData)
        {
            if (_eventQueue.TryDequeue(out eventData))
            {
                // Safely invoke the event handler
                try
                {
                    EventDequeued?.Invoke(eventData);
                }
                catch (Exception ex)
                {
                    // Log the exception but don't let it disrupt the queue operation
                    Debug.WriteLine($"Exception in EventDequeued handler: {ex.Message}");
                }

                return true;
            }

            return false;
        }
    }
}