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
}
