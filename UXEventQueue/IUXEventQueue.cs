using System;
using System.Collections.Generic;

namespace SadPumpkin.Util.UXEventQueue
{
    public interface IUXEventQueue
    {
        event Action<IUXEvent> EventEnqueued;
        event Action<IUXEvent> EventCommenced;
        event Action<IUXEvent> EventCompleted;
        
        IReadOnlyList<IUXEvent> PendingEvents { get; }
        IReadOnlyCollection<IUXEvent> RunningEvents { get; }
        
        void AddEvent(IUXEvent uxEvent);
        void TickUpdate(float deltaTimeMs);
    }
}