namespace SadPumpkin.Util.LootTable
{
    /// <summary>
    /// Implementation of ILootEntry which acts as an empty
    /// value in an ILootTable.
    /// </summary>
    public class NullLootEntry : ILootEntry
    {
        /// <summary>
        /// Weighted probability of this loot entry. Will be compared against
        /// other probabilities, with higher values being more frequent.
        /// </summary>
        public double Probability { get; set; }
        
        /// <summary>
        /// Unique loot will at most drop a single time in a table evaluation.
        /// </summary>
        public bool Unique { get; set; }
        
        /// <summary>
        /// Guaranteed loot will drop for every table evaluation, regardless
        /// of the table's count value.
        /// </summary>
        public bool Guaranteed { get; set; }

        /// <summary>
        /// Construct a new Null loot entry object with the given parameters.
        /// </summary>
        /// <param name="prob">Likelihood that this entry will appear in a table.</param>
        /// <param name="unique">Can this entry appear more than once in a result?</param>
        /// <param name="guaranteed">Does this entry appear in every result?</param>
        public NullLootEntry(double prob = 1, bool unique = false, bool guaranteed = false)
        {
            Probability = prob;
            Unique = unique;
            Guaranteed = guaranteed;
        }
    }
}