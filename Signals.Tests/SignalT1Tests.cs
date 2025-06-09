using System;
using System.Threading;
using NUnit.Framework;
using SadPumpkin.Util.Signals;

namespace SadPumpkin.Util.Signals.Tests
{
    /// <summary>
    /// Tests for the Signal&lt;T&gt; implementation with one generic parameter.
    /// </summary>
    [TestFixture]
    public class SignalT1Tests
    {
        private class TestSignal : Signal<int> { }

        [Test]
        public void Listen_WhenSignalFired_CallbackIsInvokedWithCorrectParameter()
        {
            // Arrange
            var signal = new TestSignal();
            int receivedValue = 0;
            const int expectedValue = 42;
            
            // Act
            signal.Listen(value => receivedValue = value);
            signal.Fire(expectedValue);
            
            // Assert
            Assert.AreEqual(expectedValue, receivedValue, "Callback should receive the correct parameter value");
        }
        
        [Test]
        public void Unlisten_AfterListening_CallbackIsNotInvoked()
        {
            // Arrange
            var signal = new TestSignal();
            bool callbackInvoked = false;
            Action<int> callback = _ => callbackInvoked = true;
            
            // Act
            signal.Listen(callback);
            signal.Unlisten(callback);
            signal.Fire(1);
            
            // Assert
            Assert.IsFalse(callbackInvoked, "Callback should not be invoked after unlistening");
        }
        
        [Test]
        public void Listen_MultipleTimes_AllCallbacksAreInvokedWithCorrectParameter()
        {
            // Arrange
            var signal = new TestSignal();
            int sum = 0;
            const int testValue = 10;
            
            // Act
            signal.Listen(v => sum += v);
            signal.Listen(v => sum += v);
            signal.Listen(v => sum += v);
            signal.Fire(testValue);
            
            // Assert
            Assert.AreEqual(testValue * 3, sum, "All callbacks should be invoked with the correct parameter");
        }
        
        [Test]
        public void Fire_WhenCallbackThrowsException_OtherCallbacksAreStillInvoked()
        {
            // Arrange
            var signal = new TestSignal();
            bool firstCallbackInvoked = false;
            bool secondCallbackInvoked = false;
            
            // Act
            signal.Listen(_ => firstCallbackInvoked = true);
            signal.Listen(_ => throw new Exception("Test exception"));
            signal.Listen(_ => secondCallbackInvoked = true);
            
            // This should not throw exception outside
            signal.Fire(1);
            
            // Assert
            Assert.IsTrue(firstCallbackInvoked, "First callback should be invoked");
            Assert.IsTrue(secondCallbackInvoked, "Second callback should be invoked despite exception in another callback");
        }
        
        [Test]
        public void Listen_DuringCallbackExecution_NewListenerNotInvokedUntilNextSignal()
        {
            // Arrange
            var signal = new TestSignal();
            bool newListenerInvoked = false;
            
            // Act
            signal.Listen(_ => {
                signal.Listen(__ => newListenerInvoked = true);
            });
            
            signal.Fire(1); // First fire should add the new listener but not invoke it
            
            // Assert
            Assert.IsFalse(newListenerInvoked, "New listener added during signal firing should not be invoked immediately");
            
            // Act again
            signal.Fire(1); // Second fire should invoke the new listener
            
            // Assert again
            Assert.IsTrue(newListenerInvoked, "New listener should be invoked on next signal fire");
        }
        
        [Test]
        public void Fire_WithMultithreadedListening_AllCallbacksInvoked()
        {
            // Arrange
            var signal = new TestSignal();
            int sum = 0;
            const int numThreads = 10;
            const int numCallbacksPerThread = 100;
            const int testValue = 1;
            
            object lockObject = new object();
            
            // Add callbacks from multiple threads
            using (var countdownEvent = new CountdownEvent(numThreads))
            {
                for (int i = 0; i < numThreads; i++)
                {
                    ThreadPool.QueueUserWorkItem(_ => {
                        for (int j = 0; j < numCallbacksPerThread; j++)
                        {
                            signal.Listen(v => {
                                lock (lockObject)
                                {
                                    sum += v;
                                }
                            });
                        }
                        countdownEvent.Signal();
                    });
                }
                
                // Wait for all threads to finish adding callbacks
                countdownEvent.Wait();
            }
            
            // Act
            signal.Fire(testValue);
            
            // Assert
            Assert.AreEqual(numThreads * numCallbacksPerThread * testValue, sum, 
                "All callbacks should be invoked with correct parameter when signal is fired");
        }
    }
}
