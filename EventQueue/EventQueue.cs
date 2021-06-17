using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SadPumpkin.Util.Events
{
    public class EventQueue : IEventQueue
    {
        private readonly ConcurrentQueue<IEventData> _eventQueue = new ConcurrentQueue<IEventData>();

        public event Action<IEventData> EventEnqueued;
        public event Action<IEventData> EventDequeued;

        public void EnqueueEvent(IEventData eventData)
        {
            if (eventData != null)
            {
                _eventQueue.Enqueue(eventData);

                EventEnqueued?.Invoke(eventData);
            }
        }

        public void EnqueueEvents(IEnumerable<IEventData> eventDatas)
        {
            foreach (IEventData eventData in eventDatas)
            {
                if (eventData != null)
                {
                    _eventQueue.Enqueue(eventData);

                    EventEnqueued?.Invoke(eventData);
                }
            }
        }

        public bool TryDequeueEvent(out IEventData eventData)
        {
            if (_eventQueue.TryDequeue(out eventData))
            {
                EventDequeued?.Invoke(eventData);
                return true;
            }

            return false;
        }
    }
}