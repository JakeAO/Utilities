using System;
using System.Collections.Generic;
using System.Reflection;

namespace SadPumpkin.Util.Context
{
    public class Context : IContext
    {
        private readonly IContext _baseContext = null;

        protected readonly Dictionary<Type, object> _providers = new Dictionary<Type, object>(2);
        protected readonly Dictionary<Type, object> _data = new Dictionary<Type, object>(2);

        public Context(
            IContext baseContext = null,
            IEnumerable<(Type, object)> providers = null,
            IEnumerable<(Type, object)> data = null)
        {
            _baseContext = baseContext;

            if (providers != null)
            {
                foreach ((Type type, object value) in providers)
                {
                    _providers[type] = value;
                }
            }

            if (data != null)
            {
                foreach ((Type type, object value) in data)
                {
                    _data[type] = value;
                }
            }
        }

        public T Get<T>(bool exact = false) => TryGet(out T value, exact) ? value : default;

        public bool TryGet<T>(out T value, bool exact = false)
        {
            value = default;

            Type tType = typeof(T);

            // Check for existing reference of exact type.
            if (_data.TryGetValue(tType, out var objVal) &&
                objVal is T typedVal)
            {
                value = typedVal;
                return true;
            }

            // Check for value provider of exact type.
            if (_providers.TryGetValue(tType, out var provider) &&
                provider is IValueProvider<T> typedProvider &&
                typedProvider.Get() is T providedVal)
            {
                _data[tType] = providedVal;
                value = providedVal;
                return true;
            }

            // Check for exact type within the base context.
            if (_baseContext != null &&
                _baseContext.TryGet(out T valueFromBase, exact))
            {
                value = valueFromBase;
                return true;
            }

            // Non-exact query, check base types and interfaces
            if (!exact &&
                TryGetAssignable<T>(out objVal) &&
                objVal is T assignableVal)
            {
                _data[tType] = assignableVal;
                value = assignableVal;
                return true;
            }

            return false;
        }

        private bool TryGetAssignable<T>(out object value)
        {
            Type tType = typeof(T);

            foreach (var kvp in _data)
            {
                if (tType.IsAssignableFrom(kvp.Key) &&
                    kvp.Value != null)
                {
                    value = kvp.Value;
                    return true;
                }
            }

            foreach (var kvp in _providers)
            {
                if (tType.IsAssignableFrom(kvp.Key))
                {
                    MethodInfo getValueGeneric = kvp.Value.GetType().GetMethod(nameof(IValueProvider<T>.Get));
                    if (getValueGeneric != null)
                    {
                        value = getValueGeneric.Invoke(kvp.Value, Array.Empty<object>());
                        return true;
                    }
                }
            }

            value = null;
            return false;
        }
    }
}