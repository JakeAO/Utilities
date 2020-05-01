using System;
using System.Collections.Generic;

namespace Context
{
    public class Context : IContext
    {
        private const string NIL = "NIL";

        private readonly IContext _baseContext = null;
        private readonly Dictionary<Type, Dictionary<string, object>> _data = new Dictionary<Type, Dictionary<string, object>>();

        public Context(IContext baseContext = null)
        {
            _baseContext = baseContext;
        }

        public T Get<T>(string key = null)
        {
            return TryGet(out T value, key) ? value : default;
        }

        public bool TryGet<T>(out T value, string key = null)
        {
            value = default;

            if (string.IsNullOrWhiteSpace(key))
                key = NIL;

            if (_data.TryGetValue(typeof(T), out var keyDict) &&
                keyDict.TryGetValue(key, out var objVal) &&
                objVal is T typedVal)
            {
                value = typedVal;
                return true;
            }

            if (_baseContext != null &&
                _baseContext.TryGet(out T valueFromBase, key))
            {
                value = valueFromBase;
                return true;
            }

            return false;
        }

        public void Set<T>(T value, string key = null)
        {
            if (EqualityComparer<T>.Default.Equals(value, default))
                return;

            if (string.IsNullOrWhiteSpace(key))
                key = NIL;

            if (!_data.TryGetValue(typeof(T), out var keyDict))
                _data[typeof(T)] = keyDict = new Dictionary<string, object>();

            keyDict[key] = value;
        }

        public void Clear()
        {
            _data.Clear();
        }

        public void Clear<T>()
        {
            _data.Remove(typeof(T));
        }

        public void Clear<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                key = NIL;

            if (!_data.TryGetValue(typeof(T), out var keyDict))
                return;

            keyDict.Remove(key);
        }
    }
}