namespace SadPumpkin.Util.LootTable
{
    /// <summary>
    /// Interface for all lootable items in an ILootTable.
    /// </summary>
    public interface ILootEntry
    {
        /// <summary>
        /// Weighted probability of this loot entry. Will be compared against
        /// other probabilities, with higher values being more frequent.
        /// </summary>
        double Probability { get; }
        
        /// <summary>
        /// Unique loot will at most drop a single time in a table evaluation.
        /// </summary>
        bool Unique { get; }
        
        /// <summary>
        /// Guaranteed loot will drop for every table evaluation, regardless
        /// of the table's count value.
        /// </summary>
        bool Guaranteed { get; }
    }
}