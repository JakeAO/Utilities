using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace SadPumpkin.Util.UXEventQueue.Tests
{
    [TestFixture]
    public static class UXEventQueueTests
    {
        private class TestUXEvent : IUpdateUXEvent
        {
            public event EventHandler<IUXEvent> Commenced;
            public event EventHandler<IUXEvent> Completed;

            private float _delayTime = 0f;

            public TestUXEvent(float delayTime)
            {
                _delayTime = delayTime;
            }

            public bool CanRun(IReadOnlyCollection<IUXEvent> runningEvents) => runningEvents.Count == 0;

            public void Run()
            {
                Commenced?.Invoke(this, this);

                if (_delayTime <= 0f)
                {
                    Completed?.Invoke(this, this);
                }
            }

            public void TickUpdate(float deltaTimeMs)
            {
                _delayTime -= deltaTimeMs;

                if (_delayTime <= 0f)
                {
                    Completed?.Invoke(this, this);
                }
            }
        }

        [Test]
        public static void can_create()
        {
            var testQueue = new UXEventQueue();

            Assert.IsNotNull(testQueue);
        }

        [Test]
        public static void events_added_to_pending()
        {
            var testQueue = new UXEventQueue();
            var testEvent = new TestUXEvent(10);

            Assert.IsEmpty(testQueue.PendingEvents);

            testQueue.AddEvent(testEvent);

            Assert.IsNotEmpty(testQueue.PendingEvents);
            Assert.IsTrue(testQueue.PendingEvents.Contains(testEvent));
        }

        [Test]
        public static void null_events_not_added_to_pending()
        {
            var testQueue = new UXEventQueue();
            var testEvent = (IUXEvent) null;

            Assert.IsEmpty(testQueue.PendingEvents);

            testQueue.AddEvent(testEvent);

            Assert.IsEmpty(testQueue.PendingEvents);
        }

        [Test]
        public static void unblocked_pending_events_immediately_move_to_running()
        {
            var testQueue = new UXEventQueue();
            var testEvent = new TestUXEvent(10);

            Assert.IsEmpty(testQueue.PendingEvents);
            Assert.IsEmpty(testQueue.RunningEvents);

            testQueue.AddEvent(testEvent);

            Assert.IsNotEmpty(testQueue.PendingEvents);
            Assert.IsEmpty(testQueue.RunningEvents);
            Assert.IsTrue(testQueue.PendingEvents.Contains(testEvent));

            testQueue.TickUpdate(1f);

            Assert.IsEmpty(testQueue.PendingEvents);
            Assert.IsNotEmpty(testQueue.RunningEvents);
            Assert.IsTrue(testQueue.RunningEvents.Contains(testEvent));
        }
    }
}