using System;

namespace SadPumpkin.Util.Signals
{
    /// <summary>
    /// Interface defining a parameterless signal.
    /// </summary>
    public interface ISignal
    {
        /// <summary>
        /// Send the signal to all listeners.
        /// </summary>
        void Fire();

        /// <summary>
        /// Add a listener to the signal.
        /// </summary>
        /// <param name="callback">Event called when the signal is fired.</param>
        void Listen(Action callback);

        /// <summary>
        /// Remove a listener from the signal.
        /// </summary>
        /// <param name="callback">Event called when the signal is fired.</param>
        void Unlisten(Action callback);
    }

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

    /// <summary>
    /// Interface defining a signal with two generic parameters.
    /// </summary>
    /// <typeparam name="T">Type of the first parameter.</typeparam>
    /// <typeparam name="TU">Type of the second parameter.</typeparam>
    public interface ISignal<T, TU>
    {
        /// <summary>
        /// Send the signal to all listeners.
        /// </summary>
        /// <param name="value1">First parameter.</param>
        /// <param name="value2">Second parameter.</param>
        void Fire(T value1, TU value2);

        /// <summary>
        /// Add a listener to the signal.
        /// </summary>
        /// <param name="callback">Event called when the signal is fired.</param>
        void Listen(Action<T, TU> callback);

        /// <summary>
        /// Remove a listener from the signal.
        /// </summary>
        /// <param name="callback">Event called when the signal is fired.</param>
        void Unlisten(Action<T, TU> callback);
    }

    /// <summary>
    /// Interface defining a signal with three generic parameters.
    /// </summary>
    /// <typeparam name="T">Type of the first parameter.</typeparam>
    /// <typeparam name="TU">Type of the second parameter.</typeparam>
    /// <typeparam name="TV">Type of the third parameter.</typeparam>
    public interface ISignal<T, TU, TV>
    {
        /// <summary>
        /// Send the signal to all listeners.
        /// </summary>
        /// <param name="value1">First parameter.</param>
        /// <param name="value2">Second parameter.</param>
        /// <param name="value3">Third parameter.</param>
        void Fire(T value1, TU value2, TV value3);

        /// <summary>
        /// Add a listener to the signal.
        /// </summary>
        /// <param name="callback">Event called when the signal is fired.</param>
        void Listen(Action<T, TU, TV> callback);

        /// <summary>
        /// Remove a listener from the signal.
        /// </summary>
        /// <param name="callback">Event called when the signal is fired.</param>
        void Unlisten(Action<T, TU, TV> callback);
    }
}