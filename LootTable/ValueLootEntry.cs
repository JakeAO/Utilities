namespace SadPumpkin.Util.LootTable
{
    /// <summary>
    /// Implementation of ILootEntry which contains a generically-typed
    /// value object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValueLootEntry<T> : IValueLootEntry<T>
    {
        /// <summary>
        /// Weighted probability of this loot entry. Will be compared against
        /// other probabilities, with higher values being more frequent.
        /// </summary>
        public double Probability { get; }
        
        /// <summary>
        /// Unique loot will at most drop a single time in a table evaluation.
        /// </summary>
        public bool Unique { get; }
        
        /// <summary>
        /// Guaranteed loot will drop for every table evaluation, regardless
        /// of the table's count value.
        /// </summary>
        public bool Guaranteed { get; }
        
        /// <summary>
        /// Value which this loot entry holds.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Construct a new value entry with the provided parameters.
        /// </summary>
        /// <param name="value">Value that this entry contains.</param>
        /// <param name="prob">Likelihood that this entry appears in a table's results.</param>
        /// <param name="unique">Can this entry appear more than once in a result?</param>
        /// <param name="guaranteed">Does this entry appear in every result?</param>
        public ValueLootEntry(T value, double prob = 1, bool unique = false, bool guaranteed = false)
        {
            Value = value;
            Probability = prob;
            Unique = unique;
            Guaranteed = guaranteed;
        }
    }
}