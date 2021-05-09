using System;
using System.Collections.Generic;

namespace SadPumpkin.Util.UXEventQueue
{
    public class UXEventQueue : IUXEventQueue
    {
        public event Action<IUXEvent> EventEnqueued;
        public event Action<IUXEvent> EventCommenced;
        public event Action<IUXEvent> EventCompleted;

        public IReadOnlyList<IUXEvent> PendingEvents => _pendingEvents;
        public IReadOnlyCollection<IUXEvent> RunningEvents => _runningEvents;

        private readonly List<IUXEvent> _pendingEvents = new List<IUXEvent>(100);
        private readonly List<IUXEvent> _runningEvents = new List<IUXEvent>(10);

        public void AddEvent(IUXEvent uxEvent)
        {
            if (uxEvent != null)
            {
                // Add event to our pending queue
                _pendingEvents.Add(uxEvent);
                
                // Fire off our own event
                EventEnqueued?.Invoke(uxEvent);
            }
        }

        public void TickUpdate(float deltaTimeMs)
        {
            // Call TickUpdate() on all currently running events
            TickUpdateEvents(deltaTimeMs);
            
            // Potentially move events from pending to running
            RunPendingEvents();
        }

        private void TickUpdateEvents(float deltaTimeMs)
        {
            // We iterate this in reverse to prevent issues with events
            // being removed that complete when being ticked.
            for (int i = _runningEvents.Count - 1; i >= 0; --i)
            {
                if (_runningEvents[i] is IUpdateUXEvent updateUxEvent)
                {
                    updateUxEvent.TickUpdate(deltaTimeMs);
                }
            }
        }

        private void RunPendingEvents()
        {
            while (_pendingEvents.Count > 0)
            {
                IUXEvent nextEventToRun = _pendingEvents[0];
                if (nextEventToRun.CanRun(_runningEvents))
                {
                    // Move event from pending to running
                    _pendingEvents.RemoveAt(0);
                    _runningEvents.Add(nextEventToRun);

                    // Subscribe to completion event
                    nextEventToRun.Completed += UXEvent_OnCompleted;

                    // Fire off our own event
                    EventCommenced?.Invoke(nextEventToRun);
                    
                    // Run event
                    nextEventToRun.Run();

                    continue;
                }

                break;
            }
        }

        private void UXEvent_OnCompleted(IUXEvent uxEvent)
        {
            // Remove from currently running list
            _runningEvents.Remove(uxEvent);

            // Unsubscribe from event
            uxEvent.Completed -= UXEvent_OnCompleted;

            // Fire off our own event
            EventCompleted?.Invoke(uxEvent);
        }
    }
}