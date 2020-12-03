using System;
using System.Collections.Generic;

namespace SadPumpkin.Util.LootTable
{
    /// <summary>
    /// Implementation of ILootTable which returns ILootEntries
    /// based on their probability.
    /// </summary>
    public class LootTable : ILootTable
    {
        /// <summary>
        /// Count of the ILootEntry values to return for each call to GetLoot().
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// A read-only collection of all the ILootEntries held within the table.
        /// </summary>
        public IReadOnlyCollection<ILootEntry> Entries => _allEntries;

        /// <summary>
        /// A read-only collection of all guaranteed ILootEntries held within the table.
        /// </summary>
        public IReadOnlyCollection<ILootEntry> GuaranteedEntries => _guaranteedEntries;

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
        /// Value which this loot entry holds.
        /// </summary>
        public ILootTable Value => this;

        private readonly Random _random = new Random();
        private readonly List<ILootEntry> _allEntries = new List<ILootEntry>(10);
        private readonly List<ILootEntry> _guaranteedEntries = new List<ILootEntry>(10);

        /// <summary>
        /// Construct a new LootTable with the provided parameters.
        /// </summary>
        /// <param name="count">Number of ILootEntries to return from GetLoot().</param>
        /// <param name="prob">Probability that this table is returned if inserted into another table.</param>
        /// <param name="unique">Is this table unique if inserted into another table.</param>
        /// <param name="guaranteed">Is this table guaranteed if inserted into another table.</param>
        /// <param name="entries">All entries for the table.</param>
        public LootTable(
            int count,
            double prob = 1, bool unique = false, bool guaranteed = false,
            params ILootEntry[] entries)
            : this(count, entries, prob, unique, guaranteed)
        {
        }

        /// <summary>
        /// Construct a new LootTable with the provided parameters.
        /// </summary>
        /// <param name="count">Number of ILootEntries to return from GetLoot().</param>
        /// <param name="entries">All entries for the table.</param>
        /// <param name="prob">Probability that this table is returned if inserted into another table.</param>
        /// <param name="unique">Is this table unique if inserted into another table.</param>
        /// <param name="guaranteed">Is this table guaranteed if inserted into another table.</param>
        public LootTable(
            int count,
            IReadOnlyCollection<ILootEntry> entries,
            double prob = 1, bool unique = false, bool guaranteed = false)
        {
            Count = count;
            Probability = prob;
            Unique = unique;
            Guaranteed = guaranteed;

            foreach (ILootEntry lootEntry in entries)
            {
                _allEntries.Add(lootEntry);
                if (lootEntry.Guaranteed)
                    _guaranteedEntries.Add(lootEntry);
            }
        }

        /// <summary>
        /// Generate a new, random collection of ILootEntries based on their probability.
        /// </summary>
        /// <returns>Random collection of ILootEntries</returns>
        public IReadOnlyCollection<ILootEntry> GetLoot()
        {
            void AddEntryToResult(List<ILootEntry> allResults, ILootEntry lootEntry)
            {
                if (lootEntry == null)
                    return;
                if (lootEntry is IValueLootEntry<ILootTable> lootTableEntry)
                {
                    allResults.AddRange(lootTableEntry.Value.GetLoot());
                }
                else
                {
                    allResults.Add(lootEntry);
                }
            }

            List<ILootEntry> results = new List<ILootEntry>((int) Count);

            // Add guaranteed (minimum) results
            foreach (ILootEntry lootEntry in _guaranteedEntries)
            {
                AddEntryToResult(results, lootEntry);
            }

            // Add random results
            HashSet<ILootEntry> uniques = new HashSet<ILootEntry>();
            int randomCount = Count - _guaranteedEntries.Count;
            for (int i = 0; i < randomCount; i++)
            {
                double totalProb = 0;
                foreach (ILootEntry lootEntry in _allEntries)
                {
                    if (!lootEntry.Unique ||
                        !uniques.Contains(lootEntry))
                    {
                        totalProb += lootEntry.Probability;
                    }
                }

                double hitValue = _random.NextDouble() * totalProb;
                foreach (ILootEntry lootEntry in _allEntries)
                {
                    if (hitValue <= lootEntry.Probability)
                    {
                        AddEntryToResult(results, lootEntry);
                        if (lootEntry.Unique)
                        {
                            uniques.Add(lootEntry);
                        }
                        break;
                    }
                    
                    if (!lootEntry.Unique ||
                        !uniques.Contains(lootEntry))
                    {
                        hitValue -= lootEntry.Probability;
                    }
                }
            }

            return results;
        }
    }
}