using System;
using System.Collections;
using System.Collections.Concurrent;

namespace SadPumpkin.Util.ObjectPool
{
    public class ObjectPool : IObjectPool
    {
        private readonly ConcurrentDictionary<Type, ConcurrentBag<object>> _pool = new ConcurrentDictionary<Type, ConcurrentBag<object>>();

        public T Pop<T>() where T : new()
        {
            Type type = typeof(T);

            // Get or create object pool for this type.
            if (!_pool.TryGetValue(type, out var bag))
            {
                _pool[type] = bag = new ConcurrentBag<object>();
            }

            // Pull next element from the pool, or create a new one.
            if (bag.TryTake(out var obj))
            {
                return (T) obj;
            }
            else
            {
                return new T();
            }
        }

        public void Push<T>(T obj) where T : new()
        {
            Type type = typeof(T);

            // Get or create object pool for this type.
            if (!_pool.TryGetValue(type, out var bag))
            {
                _pool[type] = bag = new ConcurrentBag<object>();
            }

            // Clear any known clearable type.
            switch (obj)
            {
                case IList asList:
                    asList.Clear();
                    break;
                case IDictionary asDictionary:
                    asDictionary.Clear();
                    break;
            }

            // Push object into the pool.
            bag.Add(obj);
        }

        public void Clear<T>()
        {
            Type type = typeof(T);

            // Get or create object pool for this type.
            if (_pool.TryGetValue(type, out var bag))
            {
                bag.Clear();
            }
        }

        public void Clear()
        {
            foreach (var poolKvp in _pool)
            {
                poolKvp.Value.Clear();
            }
        }
    }
}