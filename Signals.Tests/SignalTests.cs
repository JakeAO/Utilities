using System;
using System.Threading;
using NUnit.Framework;
using SadPumpkin.Util.Signals;

namespace SadPumpkin.Util.Signals.Tests
{
    /// <summary>
    /// Tests for the basic parameterless Signal implementation.
    /// </summary>
    [TestFixture]
    public class SignalTests
    {
        private class TestSignal : Signal { }

        [Test]
        public void Listen_WhenSignalFired_CallbackIsInvoked()
        {
            // Arrange
            var signal = new TestSignal();
            bool callbackInvoked = false;
            
            // Act
            signal.Listen(() => callbackInvoked = true);
            signal.Fire();
            
            // Assert
            Assert.IsTrue(callbackInvoked, "Callback should be invoked when signal is fired");
        }
        
        [Test]
        public void Unlisten_AfterListening_CallbackIsNotInvoked()
        {
            // Arrange
            var signal = new TestSignal();
            bool callbackInvoked = false;
            Action callback = () => callbackInvoked = true;
            
            // Act
            signal.Listen(callback);
            signal.Unlisten(callback);
            signal.Fire();
            
            // Assert
            Assert.IsFalse(callbackInvoked, "Callback should not be invoked after unlistening");
        }
        
        [Test]
        public void Listen_MultipleTimes_AllCallbacksAreInvoked()
        {
            // Arrange
            var signal = new TestSignal();
            int callCount = 0;
            
            // Act
            signal.Listen(() => callCount++);
            signal.Listen(() => callCount++);
            signal.Listen(() => callCount++);
            signal.Fire();
            
            // Assert
            Assert.AreEqual(3, callCount, "All callbacks should be invoked when signal is fired");
        }
        
        [Test]
        public void Fire_WhenCallbackThrowsException_OtherCallbacksAreStillInvoked()
        {
            // Arrange
            var signal = new TestSignal();
            bool firstCallbackInvoked = false;
            bool secondCallbackInvoked = false;
            
            // Act
            signal.Listen(() => firstCallbackInvoked = true);
            signal.Listen(() => throw new Exception("Test exception"));
            signal.Listen(() => secondCallbackInvoked = true);
            
            // This should not throw exception outside
            signal.Fire();
            
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
            signal.Listen(() => {
                signal.Listen(() => newListenerInvoked = true);
            });
            
            signal.Fire(); // First fire should add the new listener but not invoke it
            
            // Assert
            Assert.IsFalse(newListenerInvoked, "New listener added during signal firing should not be invoked immediately");
            
            // Act again
            signal.Fire(); // Second fire should invoke the new listener
            
            // Assert again
            Assert.IsTrue(newListenerInvoked, "New listener should be invoked on next signal fire");
        }
        
        [Test]
        public void Fire_WithMultipleThreads_AllCallbacksInvoked()
        {
            // Arrange
            var signal = new TestSignal();
            int callbackCount = 0;
            const int numThreads = 10;
            const int numCallbacksPerThread = 1000;
            
            // Add callbacks from multiple threads
            using (var countdownEvent = new CountdownEvent(numThreads))
            {
                for (int i = 0; i < numThreads; i++)
                {
                    ThreadPool.QueueUserWorkItem(_ => {
                        for (int j = 0; j < numCallbacksPerThread; j++)
                        {
                            signal.Listen(() => Interlocked.Increment(ref callbackCount));
                        }
                        countdownEvent.Signal();
                    });
                }
                
                // Wait for all threads to finish adding callbacks
                countdownEvent.Wait();
            }
            
            // Act
            signal.Fire();
            
            // Assert
            Assert.AreEqual(numThreads * numCallbacksPerThread, callbackCount, 
                "All callbacks should be invoked when signal is fired from multiple threads");
        }
    }
}
