namespace SadPumpkin.Util.Context
{
    public interface IContext
    {
        T Get<T>(string key = null);
        bool TryGet<T>(out T value, string key = null);
        
        void Set<T>(T value, string key = null, bool overwrite = true);

        void Clear();
        void Clear<T>();
        void Clear<T>(string key);
    }
}