using NUnit.Framework;
using SadPumpkin.Util.FilteringUtility;

namespace Tests
{
    [TestFixture]
    public class FilterTests
    {
        [Test]
        public void Ensure_Splitter_Accuracy()
        {
            Filter newFilter = new Filter("1 a 2b3c4", 'a', 'b', 'c');

            Assert.AreEqual('a', newFilter.FullFilterText[2]);
            Assert.AreEqual('b', newFilter.FullFilterText[5]);
            Assert.AreEqual('c', newFilter.FullFilterText[7]);
        }

        [Test]
        public void Ensure_General_Filter_Count()
        {
            Filter newFilter = new Filter("Test1 + Test2 + Test3", '+', '=', '&');

            Assert.AreEqual(3, newFilter.GeneralFilters.Count);
        }

        [Test]
        public void Ensure_Property_Filter_Count()
        {
            Filter newFilter = new Filter("Test1=Test1Value + Test2=Test2Value", '+', '=', '&');

            Assert.AreEqual(2, newFilter.PropertyFilters.Count);
        }
        
        [Test]
        public void Ensure_Multiple_Property_Filter_Count()
        {
            Filter newFilter = new Filter("Test1=Test1Value&Test2Value", '+', '=', '&');

            Assert.AreEqual(2, newFilter.PropertyFilters["Test1"].Count);
        }

        [Test]
        public void Ensure_Full_Text_From_General()
        {
            const string ORIGINAL_TEXT = "Test1 + Test2 + Test3";

            Filter newFilter = new Filter(ORIGINAL_TEXT, '+', '=', '&');

            Assert.AreEqual(ORIGINAL_TEXT, newFilter.FullFilterText);
        }

        [Test]
        public void Ensure_Full_Text_From_Single_Property()
        {
            const string ORIGINAL_TEXT = "Test1=Test1Value";

            Filter newFilter = new Filter(ORIGINAL_TEXT, '+', '=', '&');

            Assert.AreEqual(ORIGINAL_TEXT, newFilter.FullFilterText);
        }

        [Test]
        public void Ensure_Full_Text_From_Multiple_Properties()
        {
            const string ORIGINAL_TEXT = "Test1=Test1Value&Test2Value";

            Filter newFilter = new Filter(ORIGINAL_TEXT, '+', '=', '&');

            Assert.AreEqual(ORIGINAL_TEXT, newFilter.FullFilterText);
        }
    }
}