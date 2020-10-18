namespace SadPumpkin.Util.FilteringUtility.Property
{
    public class TextProperty : IProperty
    {
        public string Value { get; }
        public bool CanQuickFilter { get; }

        public TextProperty(string value, bool canQuickFilter = false)
        {
            Value = value;
            CanQuickFilter = canQuickFilter;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}