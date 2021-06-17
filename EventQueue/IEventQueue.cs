using System;
using System.Collections.Generic;

namespace SadPumpkin.Util.Events
{
    public interface IEventQueue
    {
        event Action<IEventData> EventEnqueued;
        event Action<IEventData> EventDequeued; 
    
        void EnqueueEvent(IEventData eventData);
        void EnqueueEvents(IEnumerable<IEventData> eventDatas);
        bool TryDequeueEvent(out IEventData eventData);
    }
}