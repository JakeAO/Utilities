namespace SadPumpkin.Util.FilteringUtility.Property
{
    public interface IProperty
    {
        bool CanQuickFilter { get; }
    }
    public interface IProperty<T> : IProperty
    {
        T Value { get; }
    }
}