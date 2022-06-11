using System;
using System.Collections.Generic;

namespace SadPumpkin.Util.Context
{
    public class Context : IContext
    {
        private readonly IContext _baseContext = null;

        private readonly Dictionary<Type, object> _providers = new Dictionary<Type, object>(10);
        private readonly Dictionary<Type, object> _data = new Dictionary<Type, object>(10);

        public Context(IContext baseContext = null)
        {
            _baseContext = baseContext;
        }

        public T Get<T>() => TryGet(out T value) ? value : default;

        public bool TryGet<T>(out T value)
        {
            value = default;

            if (_data.TryGetValue(typeof(T), out var objVal) &&
                objVal is T typedVal)
            {
                value = typedVal;
                return true;
            }

            if (_providers.TryGetValue(typeof(T), out var provider) &&
                provider is IValueProvider<T> typedProvider &&
                typedProvider.Get() is T providedVal)
            {
                _data[typeof(T)] = providedVal;
                value = providedVal;
                return true;
            }

            if (_baseContext != null &&
                _baseContext.TryGet(out T valueFromBase))
            {
                value = valueFromBase;
                return true;
            }

            return false;
        }

        public void SetProvider<T>(IValueProvider<T> valueProvider)
        {
            Type valueType = valueProvider.GetType().GetInterfaceMap(typeof(IValueProvider<T>)).InterfaceType.GenericTypeArguments[0];

            _providers[valueType] = valueProvider;
        }

        public void SetValue<T>(T value)
        {
            _data[typeof(T)] = value;
        }

        public void Clear(bool includeProviders = false)
        {
            _data.Clear();
            if (includeProviders)
            {
                _providers.Clear();
            }
        }

        public void Clear<T>(bool includeProvider = false)
        {
            _data.Remove(typeof(T));
            if (includeProvider)
            {
                _providers.Remove(typeof(T));
            }
        }
    }
}