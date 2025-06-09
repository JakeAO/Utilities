using System;
using System.Collections.Generic;

namespace SadPumpkin.Util.Events
{
    /// <summary>
    /// Interface for a queue that manages event data objects and provides notification when events are enqueued or dequeued.
    /// Used for facilitating ordered event playback in UI or other systems.
    /// </summary>
    public interface IEventQueue
    {
        /// <summary>
        /// Event fired when a new event is added to the queue.
        /// </summary>
        event Action<IEventData> EventEnqueued;
        
        /// <summary>
        /// Event fired when an event is removed from the queue.
        /// </summary>
        event Action<IEventData> EventDequeued; 
        
        /// <summary>
        /// Gets the number of events currently in the queue.
        /// </summary>
        int Count { get; }
    
        /// <summary>
        /// Adds a single event to the queue.
        /// </summary>
        /// <param name="eventData">The event data to be enqueued.</param>
        void EnqueueEvent(IEventData eventData);
        
        /// <summary>
        /// Adds multiple events to the queue.
        /// </summary>
        /// <param name="eventDatas">Collection of event data to be enqueued.</param>
        void EnqueueEvents(IEnumerable<IEventData> eventDatas);
        
        /// <summary>
        /// Attempts to remove and return the oldest event from the queue.
        /// </summary>
        /// <param name="eventData">When this method returns, contains the dequeued event data, if available; otherwise, the default value.</param>
        /// <returns>True if an event was successfully dequeued; otherwise, false.</returns>
        bool TryDequeueEvent(out IEventData eventData);
    }
}