using System;

namespace FilteringUtility.Property
{
    public class NumericProperty<T> : IProperty where T : struct, IComparable, IComparable<T>, IEquatable<T>, IFormattable
    {
        public T Value { get; }
        public bool CanQuickFilter { get; }

        public NumericProperty(T value, bool canQuickFilter = false)
        {
            Value = value;
            CanQuickFilter = canQuickFilter;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}