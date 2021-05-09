using System;
using System.Collections.Generic;

namespace SadPumpkin.Util.UXEventQueue
{
    public interface IUXEvent
    {
        event Action<IUXEvent> Commenced;
        event Action<IUXEvent> Completed;

        bool CanRun(IReadOnlyCollection<IUXEvent> runningEvents);
        void Run();
    }
}