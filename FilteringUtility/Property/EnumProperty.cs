using System;

namespace SadPumpkin.Util.FilteringUtility.Property
{
    public class EnumProperty<T> : IProperty where T : Enum
    {
        public T Value { get; }
        public bool CanQuickFilter { get; }

        public EnumProperty(T value, bool canQuickFilter = false)
        {
            Value = value;
            CanQuickFilter = canQuickFilter;
        }

        public override string ToString()
        {
            return Enum.GetName(typeof(T), Value);
        }
    }
}