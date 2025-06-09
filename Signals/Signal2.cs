using System;
using System.Collections.Generic;
using System.Threading;

namespace SadPumpkin.Util.Signals
{
    /// <summary>
    /// Abstract implementation of a signal with two generic parameters
    /// with thread safety and error handling.
    /// </summary>
    /// <typeparam name="T1">Type of the first parameter.</typeparam>
    /// <typeparam name="T2">Type of the second parameter.</typeparam>
    public abstract class Signal<T1, T2> : ISignal<T1, T2>
    {
        /// <summary>
        /// Lock object for thread synchronization
        /// </summary>
        private readonly object _lock = new object();
        
        /// <summary>
        /// Collection of event handlers for this signal
        /// </summary>
        private readonly List<Action<T1, T2>> _handlers = new List<Action<T1, T2>>();
        
        /// <summary>
        /// Reusable list for invoking handlers to avoid GC allocation
        /// </summary>
        private readonly List<Action<T1, T2>> _invocationList = new List<Action<T1, T2>>();
        
        /// <summary>
        /// Flag indicating whether the signal is currently being fired
        /// </summary>
        private bool _isFiring;
        
        /// <summary>
        /// Pending handlers to be added after firing completes
        /// </summary>
        private readonly List<Action<T1, T2>> _pendingAdd = new List<Action<T1, T2>>();
        
        /// <summary>
        /// Pending handlers to be removed after firing completes
        /// </summary>
        private readonly List<Action<T1, T2>> _pendingRemove = new List<Action<T1, T2>>();

        /// <summary>
        /// Send the signal to all listeners with thread safety and error handling.
        /// Zero allocation implementation.
        /// </summary>
        /// <param name="val1">First parameter value.</param>
        /// <param name="val2">Second parameter value.</param>
        public void Fire(T1 val1, T2 val2)
        {
            // Lock during the copy process to ensure consistency
            lock (_lock)
            {
                _isFiring = true;
                
                // Clear the invocation list and copy all handlers (no allocation)
                _invocationList.Clear();
                _invocationList.AddRange(_handlers);
            }

            // Invoke each handler outside the lock to prevent deadlocks
            int count = _invocationList.Count;
            for (int i = 0; i < count; i++)
            {
                Action<T1, T2> handler = _invocationList[i];
                try
                {
                    handler?.Invoke(val1, val2);
                }
                catch (Exception ex)
                {
                    // Log exception but continue processing other handlers
                    HandleException(ex);
                }
            }

            // Process pending handler changes after invocation is complete
            ProcessPendingChanges();
        }

        /// <summary>
        /// Add a listener to the signal in a thread-safe manner.
        /// </summary>
        /// <param name="callback">Event called when the signal is fired.</param>
        public void Listen(Action<T1, T2> callback)
        {
            if (callback == null)
                return;
                
            lock (_lock)
            {
                if (_isFiring)
                {
                    // Queue the addition for after the signal completes firing
                    _pendingAdd.Add(callback);
                }
                else
                {
                    _handlers.Add(callback);
                }
            }
        }

        /// <summary>
        /// Remove a listener from the signal in a thread-safe manner.
        /// </summary>
        /// <param name="callback">Event called when the signal is fired.</param>
        public void Unlisten(Action<T1, T2> callback)
        {
            if (callback == null)
                return;
                
            lock (_lock)
            {
                if (_isFiring)
                {
                    // Queue the removal for after the signal completes firing
                    _pendingRemove.Add(callback);
                }
                else
                {
                    _handlers.Remove(callback);
                }
            }
        }
        
        /// <summary>
        /// Process any pending handler additions or removals.
        /// </summary>
        private void ProcessPendingChanges()
        {
            lock (_lock)
            {
                _isFiring = false;
                
                // Process all pending removes
                foreach (var handler in _pendingRemove)
                {
                    _handlers.Remove(handler);
                }
                _pendingRemove.Clear();
                
                // Process all pending adds
                foreach (var handler in _pendingAdd)
                {
                    _handlers.Add(handler);
                }
                _pendingAdd.Clear();
            }
        }
        
        /// <summary>
        /// Handle exceptions that occur during event invocation.
        /// Override this method to implement custom exception handling.
        /// </summary>
        /// <param name="ex">The exception that occurred.</param>
        protected virtual void HandleException(Exception ex)
        {
            // Default implementation simply logs to console
            // Can be overridden by derived classes for custom handling
            Console.WriteLine($"Exception in signal handler: {ex.Message}");
        }
    }
}
