namespace SadPumpkin.Util.ObjectPool
{
    public interface IObjectPool
    {
        T Pop<T>() where T : new();
        void Push<T>(T obj) where T : new();

        void Clear<T>();
        void Clear();
    }
}