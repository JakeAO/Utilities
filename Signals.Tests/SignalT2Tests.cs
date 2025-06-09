using System;
using System.Threading;
using NUnit.Framework;
using SadPumpkin.Util.Signals;

namespace SadPumpkin.Util.Signals.Tests
{
    /// <summary>
    /// Tests for the Signal&lt;T1, T2&gt; implementation with two generic parameters.
    /// </summary>
    [TestFixture]
    public class SignalT2Tests
    {
        private class TestSignal : Signal<string, int> { }

        [Test]
        public void Listen_WhenSignalFired_CallbackIsInvokedWithCorrectParameters()
        {
            // Arrange
            var signal = new TestSignal();
            string receivedString = null;
            int receivedInt = 0;
            
            const string expectedString = "test";
            const int expectedInt = 42;
            
            // Act
            signal.Listen((str, num) => {
                receivedString = str;
                receivedInt = num;
            });
            
            signal.Fire(expectedString, expectedInt);
            
            // Assert
            Assert.AreEqual(expectedString, receivedString, "Callback should receive the correct first parameter");
            Assert.AreEqual(expectedInt, receivedInt, "Callback should receive the correct second parameter");
        }
        
        [Test]
        public void Unlisten_AfterListening_CallbackIsNotInvoked()
        {
            // Arrange
            var signal = new TestSignal();
            bool callbackInvoked = false;
            Action<string, int> callback = (_, __) => callbackInvoked = true;
            
            // Act
            signal.Listen(callback);
            signal.Unlisten(callback);
            signal.Fire("test", 1);
            
            // Assert
            Assert.IsFalse(callbackInvoked, "Callback should not be invoked after unlistening");
        }
        
        [Test]
        public void Listen_MultipleTimes_AllCallbacksAreInvokedWithCorrectParameters()
        {
            // Arrange
            var signal = new TestSignal();
            int sum = 0;
            string concatenated = "";
            
            const string testString = "test";
            const int testInt = 10;
            
            // Act
            signal.Listen((str, num) => {
                concatenated += str;
                sum += num;
            });
            
            signal.Listen((str, num) => {
                concatenated += str;
                sum += num;
            });
            
            signal.Fire(testString, testInt);
            
            // Assert
            Assert.AreEqual(testString + testString, concatenated, "All callbacks should be invoked with correct string parameter");
            Assert.AreEqual(testInt * 2, sum, "All callbacks should be invoked with correct int parameter");
        }
        
        [Test]
        public void Fire_WhenCallbackThrowsException_OtherCallbacksAreStillInvoked()
        {
            // Arrange
            var signal = new TestSignal();
            bool firstCallbackInvoked = false;
            bool secondCallbackInvoked = false;
            
            // Act
            signal.Listen((_, __) => firstCallbackInvoked = true);
            signal.Listen((_, __) => throw new Exception("Test exception"));
            signal.Listen((_, __) => secondCallbackInvoked = true);
            
            // This should not throw exception outside
            signal.Fire("test", 1);
            
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
            signal.Listen((_, __) => {
                signal.Listen((___, ____) => newListenerInvoked = true);
            });
            
            signal.Fire("test", 1); // First fire should add the new listener but not invoke it
            
            // Assert
            Assert.IsFalse(newListenerInvoked, "New listener added during signal firing should not be invoked immediately");
            
            // Act again
            signal.Fire("test", 1); // Second fire should invoke the new listener
            
            // Assert again
            Assert.IsTrue(newListenerInvoked, "New listener should be invoked on next signal fire");
        }
        
        [Test]
        public void Fire_WithMultithreadedListening_AllCallbacksInvoked()
        {
            // Arrange
            var signal = new TestSignal();
            int sum = 0;
            string result = "";
            const int numThreads = 5;
            const int numCallbacksPerThread = 20;
            
            const string testString = "a";
            const int testInt = 1;
            
            object lockObject = new object();
            
            // Add callbacks from multiple threads
            using (var countdownEvent = new CountdownEvent(numThreads))
            {
                for (int i = 0; i < numThreads; i++)
                {
                    ThreadPool.QueueUserWorkItem(_ => {
                        for (int j = 0; j < numCallbacksPerThread; j++)
                        {
                            signal.Listen((str, num) => {
                                lock (lockObject)
                                {
                                    result += str;
                                    sum += num;
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
            signal.Fire(testString, testInt);
            
            // Assert
            Assert.AreEqual(numThreads * numCallbacksPerThread * testInt, sum, 
                "All callbacks should receive the correct int parameter");
            Assert.AreEqual(numThreads * numCallbacksPerThread, result.Length, 
                "All callbacks should receive the correct string parameter");
        }
    }
}
