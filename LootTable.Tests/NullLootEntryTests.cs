using NUnit.Framework;

namespace SadPumpkin.Util.LootTable.Tests
{
    [TestFixture]
    public class NullLootEntryTests
    {
        [Test]
        public void can_create()
        {
            ILootEntry entry = new NullLootEntry();

            Assert.IsNotNull(entry);
            Assert.IsInstanceOf<NullLootEntry>(entry);
        }

        [Test]
        public void prob_is_set()
        {
            const double PROB = 1.2345;

            ILootEntry entry = new NullLootEntry(PROB);

            Assert.AreEqual(PROB, entry.Probability);
        }

        [Test]
        public void prob_default_is_one()
        {
            const double PROB = 1;

            ILootEntry entry = new NullLootEntry();

            Assert.AreEqual(PROB, entry.Probability);
        }

        [Test]
        public void unique_is_set()
        {
            const double PROB = 1.2345;
            const bool UNIQUE = true;

            ILootEntry entry = new NullLootEntry(PROB, UNIQUE);

            Assert.AreEqual(UNIQUE, entry.Unique);
        }

        [Test]
        public void unique_default_is_false()
        {
            const double PROB = 1.2345;
            const bool UNIQUE = false;

            ILootEntry entry = new NullLootEntry(PROB);

            Assert.AreEqual(UNIQUE, entry.Unique);
        }

        [Test]
        public void guaranteed_is_set()
        {
            const double PROB = 1.2345;
            const bool UNIQUE = true;
            const bool GUARANTEED = true;

            ILootEntry entry = new NullLootEntry(PROB, UNIQUE, GUARANTEED);

            Assert.AreEqual(GUARANTEED, entry.Guaranteed);
        }

        [Test]
        public void guaranteed_default_is_false()
        {
            const double PROB = 1.2345;
            const bool UNIQUE = true;
            const bool GUARANTEED = false;

            ILootEntry entry = new NullLootEntry(PROB, UNIQUE);

            Assert.AreEqual(GUARANTEED, entry.Guaranteed);
        }
    }
}