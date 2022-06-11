namespace SadPumpkin.Util.Context
{
    public interface IValueProvider<out T>
    {
        T Get();
    }
}