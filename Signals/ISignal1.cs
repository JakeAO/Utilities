using System;

namespace SadPumpkin.Util.Signals
{
    /// <summary>
    /// Interface defining a signal with one generic parameter.
    /// </summary>
    /// <typeparam name="T">Type of the parameter.</typeparam>
    public interface ISignal<T>
    {
        /// <summary>
        /// Send the signal to all listeners.
        /// </summary>
        /// <param name="value">First parameter.</param>
        void Fire(T value);

        /// <summary>
        /// Add a listener to the signal.
        /// </summary>
        /// <param name="callback">Event called when the signal is fired.</param>
        void Listen(Action<T> callback);

        /// <summary>
        /// Remove a listener from the signal.
        /// </summary>
        /// <param name="callback">Event called when the signal is fired.</param>
        void Unlisten(Action<T> callback);
    }
}
