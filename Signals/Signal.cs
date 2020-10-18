using System;

namespace SadPumpkin.Util.Signals
{
    public abstract class Signal : ISignal
    {
        protected event Action EventHandlers;

        public void Fire() => EventHandlers?.Invoke();
        public void Listen(Action callback) => EventHandlers += callback;
        public void Unlisten(Action callback) => EventHandlers -= callback;
    }

    public abstract class Signal<T> : ISignal<T>
    {
        protected event Action<T> EventHandlers;

        public void Fire(T val) => EventHandlers?.Invoke(val);
        public void Listen(Action<T> callback) => EventHandlers += callback;
        public void Unlisten(Action<T> callback) => EventHandlers -= callback;
    }

    public abstract class Signal<T1, T2> : ISignal<T1, T2>
    {
        protected event Action<T1, T2> EventHandlers;

        public void Fire(T1 val1, T2 val2) => EventHandlers?.Invoke(val1, val2);
        public void Listen(Action<T1, T2> callback) => EventHandlers += callback;
        public void Unlisten(Action<T1, T2> callback) => EventHandlers -= callback;
    }

    public abstract class Signal<T1, T2, T3> : ISignal<T1, T2, T3>
    {
        protected event Action<T1, T2, T3> EventHandlers;

        public void Fire(T1 val1, T2 val2, T3 val3) => EventHandlers?.Invoke(val1, val2, val3);
        public void Listen(Action<T1, T2, T3> callback) => EventHandlers += callback;
        public void Unlisten(Action<T1, T2, T3> callback) => EventHandlers -= callback;
    }
}