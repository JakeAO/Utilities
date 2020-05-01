using System.Collections.Generic;
using System.Linq;
using FilteringUtility.Property;
using NUnit.Framework;

namespace FilteringUtility.Tests
{
    [TestFixture]
    public class EntryTests
    {
        [Test]
        public void Ensure_Tracking_Key()
        {
            Entry<uint> testEntry = new Entry<uint>(123, new Dictionary<string, IProperty>()
            {
                {"Test1", new TextProperty("Test1Value")},
                {"Test2", new TextProperty("Test2Value")}
            });

            Assert.AreEqual(123, testEntry.TrackingKey);
        }

        [Test]
        public void Ensure_Property_Count()
        {
            Entry<uint> testEntry = new Entry<uint>(123, new Dictionary<string, IProperty>()
            {
                {"Test1", new TextProperty("Test1Value")},
                {"Test2", new TextProperty("Test2Value")}
            });

            Assert.AreEqual(2, testEntry.Properties.Count);
        }

        [Test]
        public void Ensure_Property_Keys()
        {
            Entry<uint> testEntry = new Entry<uint>(123, new Dictionary<string, IProperty>()
            {
                {"Test1", new TextProperty("Test1Value")},
                {"Test2", new TextProperty("Test2Value")}
            });

            Assert.IsTrue(testEntry.Properties.ContainsKey("Test1"));
            Assert.IsTrue(testEntry.Properties.ContainsKey("Test2"));
        }

        [Test]
        public void Ensure_Property_Values()
        {
            Entry<uint> testEntry = new Entry<uint>(123, new Dictionary<string, IProperty>()
            {
                {"Test1", new TextProperty("Test1Value")},
                {"Test2", new TextProperty("Test2Value")}
            });

            Assert.IsTrue(testEntry.Properties.Values.Any(x => x.ToString() == "Test1Value"));
            Assert.IsTrue(testEntry.Properties.Values.Any(x => x.ToString() == "Test2Value"));
        }

        [Test]
        public void Ensure_Combined_String()
        {
            Entry<uint> testEntry = new Entry<uint>(123, new Dictionary<string, IProperty>()
            {
                {"Test1", new TextProperty("Test1Value")},
                {"Test2", new TextProperty("Test2Value")}
            });

            Assert.AreEqual("Test1Value Test2Value", testEntry.CombinedString);
        }
    }
}