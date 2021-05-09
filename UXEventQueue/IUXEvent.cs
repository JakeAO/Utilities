using System;
using System.Collections.Generic;

namespace SadPumpkin.Util.UXEventQueue
{
    public interface IUXEvent
    {
        event EventHandler<IUXEvent> Commenced;
        event EventHandler<IUXEvent> Completed;

        bool CanRun(IReadOnlyCollection<IUXEvent> runningEvents);
        void Run();
    }
}