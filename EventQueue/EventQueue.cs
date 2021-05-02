using System.Collections.Concurrent;
using System.Collections.Generic;

namespace SadPumpkin.Util.Events
{
    public class EventQueue : IEventQueue
    {
        private readonly ConcurrentQueue<IEventData> _eventQueue = new ConcurrentQueue<IEventData>();

        public void EnqueueEvent(IEventData eventData) => _eventQueue.Enqueue(eventData);

        public void EnqueueEvents(IEnumerable<IEventData> eventDatas)
        {
            foreach (IEventData eventData in eventDatas)
            {
                _eventQueue.Enqueue(eventData);
            }
        }

        public bool TryDequeueEvent(out IEventData eventData) => _eventQueue.TryDequeue(out eventData);
    }
}