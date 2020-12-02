using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace SadPumpkin.Util.LootTable.Tests
{
    [TestFixture]
    public class LootTableTests
    {
        [Test]
        public void can_create()
        {
            ILootTable table = new LootTable(1);

            Assert.IsNotNull(table);
            Assert.IsInstanceOf<LootTable>(table);
            Assert.IsInstanceOf<IValueLootEntry<ILootTable>>(table);
            Assert.IsInstanceOf<ILootEntry>(table);
        }

        [Test]
        public void count_is_set()
        {
            const int COUNT = 15;

            ILootTable table = new LootTable(COUNT);

            Assert.AreEqual(COUNT, table.Count);
        }

        [Test]
        public void value_is_set()
        {
            const int COUNT = 15;

            ILootTable entry = new LootTable(COUNT);

            Assert.AreEqual(entry, entry.Value);
        }

        [Test]
        public void prob_is_set()
        {
            const int COUNT = 15;
            const double PROB = 1.2345;

            ILootTable entry = new LootTable(COUNT, PROB);

            Assert.AreEqual(PROB, entry.Probability);
        }

        [Test]
        public void prob_default_is_one()
        {
            const int COUNT = 15;
            const double PROB = 1;

            ILootTable entry = new LootTable(COUNT);

            Assert.AreEqual(PROB, entry.Probability);
        }

        [Test]
        public void unique_is_set()
        {
            const int COUNT = 15;
            const double PROB = 1.2345;
            const bool UNIQUE = true;

            ILootTable entry = new LootTable(COUNT, PROB, UNIQUE);

            Assert.AreEqual(UNIQUE, entry.Unique);
        }

        [Test]
        public void unique_default_is_false()
        {
            const int COUNT = 15;
            const double PROB = 1.2345;
            const bool UNIQUE = false;

            ILootTable entry = new LootTable(COUNT, PROB);

            Assert.AreEqual(UNIQUE, entry.Unique);
        }

        [Test]
        public void guaranteed_is_set()
        {
            const int COUNT = 15;
            const double PROB = 1.2345;
            const bool UNIQUE = true;
            const bool GUARANTEED = true;

            ILootTable entry = new LootTable(COUNT, PROB, UNIQUE, GUARANTEED);

            Assert.AreEqual(GUARANTEED, entry.Guaranteed);
        }

        [Test]
        public void guaranteed_default_is_false()
        {
            const int COUNT = 15;
            const double PROB = 1.2345;
            const bool UNIQUE = true;
            const bool GUARANTEED = false;

            ILootTable entry = new LootTable(COUNT, PROB, UNIQUE);

            Assert.AreEqual(GUARANTEED, entry.Guaranteed);
        }

        [Test]
        public void entries_is_set()
        {
            const int COUNT = 15;
            ILootEntry[] LOOT_ENTRIES = new ILootEntry[]
            {
                new NullLootEntry(),
                new ValueLootEntry<string>("test"),
            };

            ILootTable table = new LootTable(COUNT, LOOT_ENTRIES);

            foreach (ILootEntry expectedEntry in LOOT_ENTRIES)
            {
                Assert.IsTrue(table.Entries.Contains(expectedEntry));
            }
        }

        [Test]
        public void guaranteed_entries_is_set()
        {
            const int COUNT = 15;
            ILootEntry[] LOOT_ENTRIES = new ILootEntry[]
            {
                new NullLootEntry(guaranteed: true),
                new ValueLootEntry<string>("test1"),
                new ValueLootEntry<string>("test2", guaranteed: true),
            };

            ILootTable table = new LootTable(COUNT, LOOT_ENTRIES);

            foreach (ILootEntry expectedEntry in LOOT_ENTRIES)
            {
                if (expectedEntry.Guaranteed)
                {
                    Assert.IsTrue(table.GuaranteedEntries.Contains(expectedEntry));
                }
                else
                {
                    Assert.IsFalse(table.GuaranteedEntries.Contains(expectedEntry));
                }
            }
        }

        [Test]
        public void get_loot_returns_proper_count_when_no_guaranteed()
        {
            ILootEntry[] LOOT_ENTRIES = new ILootEntry[]
            {
                new NullLootEntry(),
                new ValueLootEntry<string>("test1"),
                new ValueLootEntry<string>("test2"),
            };

            LootTable table = new LootTable(1, LOOT_ENTRIES);

            table.Count = 1;
            Assert.AreEqual(table.Count, table.GetLoot().Count);

            table.Count = 3;
            Assert.AreEqual(table.Count, table.GetLoot().Count);

            table.Count = 5;
            Assert.AreEqual(table.Count, table.GetLoot().Count);
        }

        [Test]
        public void get_loot_returns_proper_count_when_guaranteed()
        {
            ILootEntry[] LOOT_ENTRIES = new ILootEntry[]
            {
                new NullLootEntry(),
                new ValueLootEntry<string>("test1"),
                new ValueLootEntry<string>("test2"),
                new ValueLootEntry<string>("test3", guaranteed: true),
                new ValueLootEntry<string>("test4", guaranteed: true),
            };

            LootTable table = new LootTable(1, LOOT_ENTRIES);

            table.Count = 1;
            Assert.AreEqual(2, table.GetLoot().Count);

            table.Count = 3;
            Assert.AreEqual(table.Count, table.GetLoot().Count);

            table.Count = 5;
            Assert.AreEqual(table.Count, table.GetLoot().Count);
        }

        [Test]
        public void get_loot_returns_only_one_unique_entry()
        {
            const int COUNT = 15;
            ILootEntry UNIQUE_ENTRY = new ValueLootEntry<string>("test4", prob: 100, unique: true);
            ILootEntry[] LOOT_ENTRIES = new ILootEntry[]
            {
                new NullLootEntry(),
                new ValueLootEntry<string>("test1"),
                new ValueLootEntry<string>("test2"),
                new ValueLootEntry<string>("test3"),
                UNIQUE_ENTRY,
            };

            LootTable table = new LootTable(COUNT, LOOT_ENTRIES);

            IReadOnlyCollection<ILootEntry> results = table.GetLoot();

            Assert.LessOrEqual(1, results.Count(x => x == UNIQUE_ENTRY));
        }
    }
}