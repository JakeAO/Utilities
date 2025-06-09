using System;

namespace SadPumpkin.Util.Signals
{
    /// <summary>
    /// Interface defining a signal with three generic parameters.
    /// </summary>
    /// <typeparam name="T1">Type of the first parameter.</typeparam>
    /// <typeparam name="T2">Type of the second parameter.</typeparam>
    /// <typeparam name="T3">Type of the third parameter.</typeparam>
    public interface ISignal<T1, T2, T3>
    {
        /// <summary>
        /// Send the signal to all listeners.
        /// </summary>
        /// <param name="value1">First parameter.</param>
        /// <param name="value2">Second parameter.</param>
        /// <param name="value3">Third parameter.</param>
        void Fire(T1 value1, T2 value2, T3 value3);

        /// <summary>
        /// Add a listener to the signal.
        /// </summary>
        /// <param name="callback">Event called when the signal is fired.</param>
        void Listen(Action<T1, T2, T3> callback);

        /// <summary>
        /// Remove a listener from the signal.
        /// </summary>
        /// <param name="callback">Event called when the signal is fired.</param>
        void Unlisten(Action<T1, T2, T3> callback);
    }
}
