using System;

namespace SadPumpkin.Util.Context
{
    public class MutableContext : Context, IMutableContext
    {
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