﻿using System;

namespace SadPumpkin.Util.Signals
{
    public interface ISignal
    {
        void Fire();
        void Listen(Action callback);
        void Unlisten(Action callback);
    }
    
    public interface ISignal<T>
    {
        void Fire(T value);
        void Listen(Action<T> callback);
        void Unlisten(Action<T> callback);
    }

    public interface ISignal<T, TU>
    {
        void Fire(T value1, TU value2);
        void Listen(Action<T, TU> callback);
        void Unlisten(Action<T, TU> callback);
    }

    public interface ISignal<T, TU, TV>
    {
        void Fire(T value1, TU value2, TV value3);
        void Listen(Action<T, TU, TV> callback);
        void Unlisten(Action<T, TU, TV> callback);
    }
}