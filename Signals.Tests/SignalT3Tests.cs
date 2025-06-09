using System;
using System.Threading;
using NUnit.Framework;
using SadPumpkin.Util.Signals;

namespace SadPumpkin.Util.Signals.Tests
{
    /// <summary>
    /// Tests for the Signal&lt;T1, T2, T3&gt; implementation with three generic parameters.
    /// </summary>
    [TestFixture]
    public class SignalT3Tests
    {
        private class TestSignal : Signal<string, int, bool> { }

        [Test]
        public void Listen_WhenSignalFired_CallbackIsInvokedWithCorrectParameters()
        {
            // Arrange
            var signal = new TestSignal();
            string receivedString = null;
            int receivedInt = 0;
            bool receivedBool = false;
            
            const string expectedString = "test";
            const int expectedInt = 42;
            const bool expectedBool = true;
            
            // Act
            signal.Listen((str, num, flag) => {
                receivedString = str;
                receivedInt = num;
                receivedBool = flag;
            });
            
            signal.Fire(expectedString, expectedInt, expectedBool);
            
            // Assert
            Assert.AreEqual(expectedString, receivedString, "Callback should receive the correct first parameter");
            Assert.AreEqual(expectedInt, receivedInt, "Callback should receive the correct second parameter");
            Assert.AreEqual(expectedBool, receivedBool, "Callback should receive the correct third parameter");
        }
        
        [Test]
        public void Unlisten_AfterListening_CallbackIsNotInvoked()
        {
            // Arrange
            var signal = new TestSignal();
            bool callbackInvoked = false;
            Action<string, int, bool> callback = (_, __, ___) => callbackInvoked = true;
            
            // Act
            signal.Listen(callback);
            signal.Unlisten(callback);
            signal.Fire("test", 1, true);
            
            // Assert
            Assert.IsFalse(callbackInvoked, "Callback should not be invoked after unlistening");
        }
        
        [Test]
        public void Listen_MultipleTimes_AllCallbacksAreInvokedWithCorrectParameters()
        {
            // Arrange
            var signal = new TestSignal();
            int count = 0;
            
            // Act
            signal.Listen((_, __, flag) => {
                if (flag) count++;
            });
            
            signal.Listen((_, __, flag) => {
                if (flag) count++;
            });
            
            signal.Fire("test", 10, true);
            
            // Assert
            Assert.AreEqual(2, count, "All callbacks should be invoked with correct parameters");
        }
        
        [Test]
        public void Fire_WhenCallbackThrowsException_OtherCallbacksAreStillInvoked()
        {
            // Arrange
            var signal = new TestSignal();
            bool firstCallbackInvoked = false;
            bool secondCallbackInvoked = false;
            
            // Act
            signal.Listen((_, __, ___) => firstCallbackInvoked = true);
            signal.Listen((_, __, ___) => throw new Exception("Test exception"));
            signal.Listen((_, __, ___) => secondCallbackInvoked = true);
            
            // This should not throw exception outside
            signal.Fire("test", 1, true);
            
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
            signal.Listen((_, __, ___) => {
                signal.Listen((____, _____, ______) => newListenerInvoked = true);
            });
            
            signal.Fire("test", 1, true); // First fire should add the new listener but not invoke it
            
            // Assert
            Assert.IsFalse(newListenerInvoked, "New listener added during signal firing should not be invoked immediately");
            
            // Act again
            signal.Fire("test", 1, true); // Second fire should invoke the new listener
            
            // Assert again
            Assert.IsTrue(newListenerInvoked, "New listener should be invoked on next signal fire");
        }
        
        [Test]
        public void Fire_WithMultithreadedListening_AllCallbacksInvoked()
        {
            // Arrange
            var signal = new TestSignal();
            int invokeCount = 0;
            const int numThreads = 4;
            const int numCallbacksPerThread = 25;
            
            object lockObject = new object();
            
            // Add callbacks from multiple threads
            using (var countdownEvent = new CountdownEvent(numThreads))
            {
                for (int i = 0; i < numThreads; i++)
                {
                    ThreadPool.QueueUserWorkItem(_ => {
                        for (int j = 0; j < numCallbacksPerThread; j++)
                        {
                            signal.Listen((__, ___, flag) => {
                                if (flag)
                                {
                                    lock (lockObject)
                                    {
                                        invokeCount++;
                                    }
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
            signal.Fire("test", 1, true);
            
            // Assert
            Assert.AreEqual(numThreads * numCallbacksPerThread, invokeCount, 
                "All callbacks should be invoked with correct parameters");
        }
    }
}
