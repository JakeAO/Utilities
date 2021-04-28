using System;

namespace SadPumpkin.Util.Signals
{
    /// <summary>
    /// Abstract implementation of a parameterless signal.
    /// </summary>
    public abstract class Signal : ISignal
    {
        protected event Action EventHandlers;

        /// <summary>
        /// Send the signal to all listeners.
        /// </summary>
        public void Fire() => EventHandlers?.Invoke();

        /// <summary>
        /// Add a listener to the signal.
        /// </summary>
        /// <param name="callback">Event called when the signal is fired.</param>
        public void Listen(Action callback) => EventHandlers += callback;

        /// <summary>
        /// Remove a listener from the signal.
        /// </summary>
        /// <param name="callback">Event called when the signal is fired.</param>
        public void Unlisten(Action callback) => EventHandlers -= callback;
    }

    /// <summary>
    /// Abstract implementation of a signal with one generic parameter.
    /// </summary>
    /// <typeparam name="T">Type of the parameter.</typeparam>
    public abstract class Signal<T> : ISignal<T>
    {
        protected event Action<T> EventHandlers;

        /// <summary>
        /// Send the signal to all listeners.
        /// </summary>
        /// <param name="value">First parameter.</param>
        public void Fire(T val) => EventHandlers?.Invoke(val);

        /// <summary>
        /// Add a listener to the signal.
        /// </summary>
        /// <param name="callback">Event called when the signal is fired.</param>
        public void Listen(Action<T> callback) => EventHandlers += callback;

        /// <summary>
        /// Remove a listener from the signal.
        /// </summary>
        /// <param name="callback">Event called when the signal is fired.</param>
        public void Unlisten(Action<T> callback) => EventHandlers -= callback;
    }

    /// <summary>
    /// Abstract implementation of a signal with two generic parameters.
    /// </summary>
    /// <typeparam name="T">Type of the first parameter.</typeparam>
    /// <typeparam name="TU">Type of the second parameter.</typeparam>
    public abstract class Signal<T1, T2> : ISignal<T1, T2>
    {
        protected event Action<T1, T2> EventHandlers;

        /// <summary>
        /// Send the signal to all listeners.
        /// </summary>
        /// <param name="value1">First parameter.</param>
        /// <param name="value2">Second parameter.</param>
        public void Fire(T1 val1, T2 val2) => EventHandlers?.Invoke(val1, val2);

        /// <summary>
        /// Add a listener to the signal.
        /// </summary>
        /// <param name="callback">Event called when the signal is fired.</param>
        public void Listen(Action<T1, T2> callback) => EventHandlers += callback;

        /// <summary>
        /// Remove a listener from the signal.
        /// </summary>
        /// <param name="callback">Event called when the signal is fired.</param>
        public void Unlisten(Action<T1, T2> callback) => EventHandlers -= callback;
    }

    /// <summary>
    /// Abstract implementation of a signal with three generic parameters.
    /// </summary>
    /// <typeparam name="T">Type of the first parameter.</typeparam>
    /// <typeparam name="TU">Type of the second parameter.</typeparam>
    /// <typeparam name="TV">Type of the third parameter.</typeparam>
    public abstract class Signal<T1, T2, T3> : ISignal<T1, T2, T3>
    {
        protected event Action<T1, T2, T3> EventHandlers;

        /// <summary>
        /// Send the signal to all listeners.
        /// </summary>
        /// <param name="value1">First parameter.</param>
        /// <param name="value2">Second parameter.</param>
        /// <param name="value3">Third parameter.</param>
        public void Fire(T1 val1, T2 val2, T3 val3) => EventHandlers?.Invoke(val1, val2, val3);

        /// <summary>
        /// Add a listener to the signal.
        /// </summary>
        /// <param name="callback">Event called when the signal is fired.</param>
        public void Listen(Action<T1, T2, T3> callback) => EventHandlers += callback;

        /// <summary>
        /// Remove a listener from the signal.
        /// </summary>
        /// <param name="callback">Event called when the signal is fired.</param>
        public void Unlisten(Action<T1, T2, T3> callback) => EventHandlers -= callback;
    }
}