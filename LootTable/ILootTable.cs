using System.Collections.Generic;

namespace SadPumpkin.Util.LootTable
{
    /// <summary>
    /// Interface for a randomized loot table which returns ILootEntries
    /// based on their probability.
    /// </summary>
    public interface ILootTable : IValueLootEntry<ILootTable>
    {
        /// <summary>
        /// Count of the ILootEntry values to return for each call to GetLoot().
        /// </summary>
        int Count { get; }
        
        /// <summary>
        /// A read-only collection of all the ILootEntries held within the table.
        /// </summary>
        IReadOnlyCollection<ILootEntry> Entries { get; }

        /// <summary>
        /// Generate a new, random collection of ILootEntries based on their probability.
        /// </summary>
        /// <returns>Random collection of ILootEntries</returns>
        IReadOnlyCollection<ILootEntry> GetLoot();
    }
}