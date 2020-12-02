using NUnit.Framework;

namespace SadPumpkin.Util.LootTable.Tests
{
    [TestFixture]
    public class ValueLootEntryTests
    {
        [Test]
        public void can_create()
        {
            ILootEntry entry = new ValueLootEntry<string>("test");

            Assert.IsNotNull(entry);
            Assert.IsInstanceOf<ValueLootEntry<string>>(entry);
        }

        [Test]
        public void value_is_set()
        {
            const string VALUE = "test";

            IValueLootEntry<string> entry = new ValueLootEntry<string>(VALUE);

            Assert.AreEqual(VALUE, entry.Value);
        }

        [Test]
        public void prob_is_set()
        {
            const string VALUE = "test";
            const double PROB = 1.2345;

            ILootEntry entry = new ValueLootEntry<string>(VALUE, PROB);

            Assert.AreEqual(PROB, entry.Probability);
        }

        [Test]
        public void prob_default_is_one()
        {
            const string VALUE = "test";
            const double PROB = 1;

            ILootEntry entry = new ValueLootEntry<string>(VALUE);

            Assert.AreEqual(PROB, entry.Probability);
        }

        [Test]
        public void unique_is_set()
        {
            const string VALUE = "test";
            const double PROB = 1.2345;
            const bool UNIQUE = true;

            ILootEntry entry = new ValueLootEntry<string>(VALUE, PROB, UNIQUE);

            Assert.AreEqual(UNIQUE, entry.Unique);
        }

        [Test]
        public void unique_default_is_false()
        {
            const string VALUE = "test";
            const double PROB = 1.2345;
            const bool UNIQUE = false;

            ILootEntry entry = new ValueLootEntry<string>(VALUE, PROB);

            Assert.AreEqual(UNIQUE, entry.Unique);
        }

        [Test]
        public void guaranteed_is_set()
        {
            const string VALUE = "test";
            const double PROB = 1.2345;
            const bool UNIQUE = true;
            const bool GUARANTEED = true;

            ILootEntry entry = new ValueLootEntry<string>(VALUE, PROB, UNIQUE, GUARANTEED);

            Assert.AreEqual(GUARANTEED, entry.Guaranteed);
        }

        [Test]
        public void guaranteed_default_is_false()
        {
            const string VALUE = "test";
            const double PROB = 1.2345;
            const bool UNIQUE = true;
            const bool GUARANTEED = false;

            ILootEntry entry = new ValueLootEntry<string>(VALUE, PROB, UNIQUE);

            Assert.AreEqual(GUARANTEED, entry.Guaranteed);
        }
    }
}