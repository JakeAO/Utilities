using System.Collections.Generic;

namespace SadPumpkin.Util.Events
{
    public interface IEventQueue
    {
        void EnqueueEvent(IEventData eventData);
        void EnqueueEvents(IEnumerable<IEventData> eventDatas);
        bool TryDequeueEvent(out IEventData eventData);
    }
}