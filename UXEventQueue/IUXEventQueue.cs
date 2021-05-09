using System;
using System.Collections.Generic;

namespace SadPumpkin.Util.UXEventQueue
{
    public interface IUXEventQueue
    {
        event EventHandler<IUXEvent> EventEnqueued;
        event EventHandler<IUXEvent> EventCommenced;
        event EventHandler<IUXEvent> EventCompleted;
        
        IReadOnlyList<IUXEvent> PendingEvents { get; }
        IReadOnlyCollection<IUXEvent> RunningEvents { get; }
        
        void AddEvent(IUXEvent uxEvent);
        void TickUpdate(float deltaTimeMs);
    }
}