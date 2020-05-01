namespace FilteringUtility.Property
{
    public class BooleanProperty : IProperty
    {
        private const string TRUE_VAL = "True";
        private const string FALSE_VAL = "False";
        
        public bool Value { get; }
        public bool CanQuickFilter { get; }

        public BooleanProperty(bool value, bool canQuickFilter = false)
        {
            Value = value;
            CanQuickFilter = canQuickFilter;
        }

        public override string ToString()
        {
            return Value ? TRUE_VAL : FALSE_VAL;
        }
    }
}