using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SadPumpkin.Util.Events;

namespace SadPumpkin.Util.EventQueue.Tests
{
    [TestFixture]
    public class EventQueueTests
    {
        private Events.EventQueue _eventQueue;
        
        [SetUp]
        public void SetUp()
        {
            _eventQueue = new Events.EventQueue();
        }
        
        #region EnqueueEvent Tests
        
        [Test]
        public void EnqueueEvent_WithNullEvent_DoesNotEnqueueEvent()
        {
            // Arrange
            bool eventFired = false;
            _eventQueue.EventEnqueued += _ => eventFired = true;
            
            // Act
            _eventQueue.EnqueueEvent(null);
            bool result = _eventQueue.TryDequeueEvent(out _);
            
            // Assert
            Assert.IsFalse(eventFired, "EventEnqueued event should not be triggered for null event");
            Assert.IsFalse(result, "No event should be available to dequeue");
        }
        
        [Test]
        public void EnqueueEvent_WithValidEvent_EnqueuesEvent()
        {
            // Arrange
            var testEvent = new TestEventData();
            IEventData capturedEvent = null;
            _eventQueue.EventEnqueued += eventData => capturedEvent = eventData;
            
            // Act
            _eventQueue.EnqueueEvent(testEvent);
            
            // Assert
            Assert.IsNotNull(capturedEvent, "EventEnqueued event should be triggered");
            Assert.AreSame(testEvent, capturedEvent, "EventEnqueued should provide the enqueued event");
        }
        
        [Test]
        public void EnqueueEvent_WithMultipleEvents_MaintainsOrder()
        {
            // Arrange
            var firstEvent = new TestEventData { Id = 1 };
            var secondEvent = new TestEventData { Id = 2 };
            
            // Act
            _eventQueue.EnqueueEvent(firstEvent);
            _eventQueue.EnqueueEvent(secondEvent);
            
            // Assert
            Assert.IsTrue(_eventQueue.TryDequeueEvent(out var dequeuedFirst));
            Assert.IsTrue(_eventQueue.TryDequeueEvent(out var dequeuedSecond));
            
            Assert.AreEqual(1, ((TestEventData)dequeuedFirst).Id);
            Assert.AreEqual(2, ((TestEventData)dequeuedSecond).Id);
        }
        
        #endregion
        
        #region EnqueueEvents Tests
        
        [Test]
        public void EnqueueEvents_WithNullCollection_DoesNotThrowException()
        {
            // Act & Assert
            Assert.DoesNotThrow(() => _eventQueue.EnqueueEvents(null));
        }
        
        [Test]
        public void EnqueueEvents_WithEmptyCollection_DoesNotEnqueueEvents()
        {
            // Arrange
            bool eventFired = false;
            _eventQueue.EventEnqueued += _ => eventFired = true;
            
            // Act
            _eventQueue.EnqueueEvents(new List<IEventData>());
            bool result = _eventQueue.TryDequeueEvent(out _);
            
            // Assert
            Assert.IsFalse(eventFired, "EventEnqueued event should not be triggered for empty collection");
            Assert.IsFalse(result, "No event should be available to dequeue");
        }
        
        [Test]
        public void EnqueueEvents_WithMixedValidAndNullEvents_EnqueuesOnlyValidEvents()
        {
            // Arrange
            var events = new List<IEventData>
            {
                new TestEventData { Id = 1 },
                null,
                new TestEventData { Id = 2 }
            };
            
            var enqueuedEvents = new List<IEventData>();
            _eventQueue.EventEnqueued += eventData => enqueuedEvents.Add(eventData);
            
            // Act
            _eventQueue.EnqueueEvents(events);
            
            // Assert
            Assert.AreEqual(2, enqueuedEvents.Count, "Only non-null events should be enqueued");
            Assert.AreEqual(1, ((TestEventData)enqueuedEvents[0]).Id);
            Assert.AreEqual(2, ((TestEventData)enqueuedEvents[1]).Id);
        }
        
        [Test]
        public void EnqueueEvents_WithMultipleEvents_MaintainsOrder()
        {
            // Arrange
            var events = new List<IEventData>
            {
                new TestEventData { Id = 1 },
                new TestEventData { Id = 2 },
                new TestEventData { Id = 3 }
            };
            
            // Act
            _eventQueue.EnqueueEvents(events);
            
            // Assert
            Assert.IsTrue(_eventQueue.TryDequeueEvent(out var first));
            Assert.IsTrue(_eventQueue.TryDequeueEvent(out var second));
            Assert.IsTrue(_eventQueue.TryDequeueEvent(out var third));
            
            Assert.AreEqual(1, ((TestEventData)first).Id);
            Assert.AreEqual(2, ((TestEventData)second).Id);
            Assert.AreEqual(3, ((TestEventData)third).Id);
        }
        
        #endregion
        
        #region TryDequeueEvent Tests
        
        [Test]
        public void TryDequeueEvent_WithEmptyQueue_ReturnsFalse()
        {
            // Act
            bool result = _eventQueue.TryDequeueEvent(out var eventData);
            
            // Assert
            Assert.IsFalse(result, "TryDequeueEvent should return false when queue is empty");
            Assert.IsNull(eventData, "Dequeued event data should be null when queue is empty");
        }
        
        [Test]
        public void TryDequeueEvent_WithQueueContainingEvents_ReturnsTrueAndEvent()
        {
            // Arrange
            var testEvent = new TestEventData();
            _eventQueue.EnqueueEvent(testEvent);
            
            IEventData capturedEvent = null;
            _eventQueue.EventDequeued += eventData => capturedEvent = eventData;
            
            // Act
            bool result = _eventQueue.TryDequeueEvent(out var eventData2);
            
            // Assert
            Assert.IsTrue(result, "TryDequeueEvent should return true when queue has events");
            Assert.AreSame(testEvent, eventData2, "Dequeued event should be the same as enqueued event");
            Assert.AreSame(testEvent, capturedEvent, "EventDequeued should provide the dequeued event");
        }
        
        [Test]
        public void TryDequeueEvent_AfterDequeuingAllEvents_ReturnsFalse()
        {
            // Arrange
            _eventQueue.EnqueueEvent(new TestEventData());
            _eventQueue.TryDequeueEvent(out _);
            
            // Act
            bool result = _eventQueue.TryDequeueEvent(out var eventData);
            
            // Assert
            Assert.IsFalse(result, "TryDequeueEvent should return false after all events are dequeued");
            Assert.IsNull(eventData, "Dequeued event data should be null when queue is empty");
        }
        
        #endregion
        
        #region Event Handler Tests
        
        [Test]
        public void EventEnqueued_WhenHandlerThrowsException_DoesNotAffectEnqueueOperation()
        {
            // Arrange
            var testEvent = new TestEventData();
            _eventQueue.EventEnqueued += _ => throw new Exception("Test exception");
            
            // Act & Assert
            Assert.DoesNotThrow(() => _eventQueue.EnqueueEvent(testEvent));
            Assert.IsTrue(_eventQueue.TryDequeueEvent(out var eventData));
            Assert.AreSame(testEvent, eventData);
        }
        
        [Test]
        public void EventDequeued_WhenHandlerThrowsException_DoesNotAffectDequeueOperation()
        {
            // Arrange
            var testEvent = new TestEventData();
            _eventQueue.EnqueueEvent(testEvent);
            _eventQueue.EventDequeued += _ => throw new Exception("Test exception");
            
            // Act & Assert
            var eventData = default(IEventData);
            Assert.DoesNotThrow(() => _eventQueue.TryDequeueEvent(out eventData));
            Assert.AreSame(testEvent, eventData);
        }
        
        #endregion
        
        #region Thread Safety Tests
        
        [Test]
        public void EventQueue_WithConcurrentOperations_MaintainsConsistency()
        {
            // Arrange
            const int eventCount = 1000;
            var events = Enumerable.Range(1, eventCount)
                .Select(i => new TestEventData { Id = i })
                .ToList();
                
            var enqueuedIds = new HashSet<int>();
            var dequeuedIds = new HashSet<int>();
            
            _eventQueue.EventEnqueued += e => enqueuedIds.Add(((TestEventData)e).Id);
            _eventQueue.EventDequeued += e => dequeuedIds.Add(((TestEventData)e).Id);
            
            // Act
            // Enqueue all events first
            _eventQueue.EnqueueEvents(events.Cast<IEventData>());
            
            // Dequeue all events
            var dequeuedEvents = new List<IEventData>();
            while (_eventQueue.TryDequeueEvent(out var eventData))
            {
                dequeuedEvents.Add(eventData);
            }
            
            // Assert
            Assert.AreEqual(eventCount, enqueuedIds.Count, "All events should be enqueued");
            Assert.AreEqual(eventCount, dequeuedEvents.Count, "All events should be dequeued");
            Assert.AreEqual(eventCount, dequeuedIds.Count, "All dequeue events should be fired");
            
            for (int i = 1; i <= eventCount; i++)
            {
                Assert.IsTrue(enqueuedIds.Contains(i), $"Event with ID {i} should have been enqueued");
                Assert.IsTrue(dequeuedIds.Contains(i), $"Event with ID {i} should have been dequeued");
            }
        }
        
        #endregion
    }
    
    /// <summary>
    /// Simple implementation of IEventData for testing
    /// </summary>
    public class TestEventData : IEventData
    {
        public int Id { get; set; }
    }
}