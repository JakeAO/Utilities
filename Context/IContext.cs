namespace SadPumpkin.Util.Context
{
    public interface IContext
    {
        T Get<T>();
        bool TryGet<T>(out T value);

        void SetProvider<T>(IValueProvider<T> valueProvider);
        void SetValue<T>(T value);

        void Clear(bool includeProviders = false);
        void Clear<T>(bool includeProvider = false);
    }
}