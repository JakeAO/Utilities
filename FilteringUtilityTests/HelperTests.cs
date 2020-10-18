using System;
using System.Collections.Generic;
using NUnit.Framework;
using SadPumpkin.Util.FilteringUtility;
using SadPumpkin.Util.FilteringUtility.Property;

namespace Tests
{
    [TestFixture]
    public class HelperTests
    {
        [Test]
        public void Ensure_List_Conversion()
        {
            IReadOnlyList<string> newList = new[] {"Test1", "Test2", "Test3"};

            string convertedText = Helpers.GetListText(newList);

            Assert.AreEqual("Test1 Test2 Test3", convertedText);
        }

        [Test]
        public void Ensure_Enum_List_Conversion()
        {
            IReadOnlyList<TypeCode> newList = new[] {TypeCode.Boolean, TypeCode.Byte, TypeCode.Char};

            string convertedText = Helpers.GetEnumListText(newList);

            Assert.AreEqual("Boolean Byte Char", convertedText);
        }

        [Test]
        public void Ensure_Add_Property_No_Key()
        {
            Dictionary<string, IProperty> props = new Dictionary<string, IProperty>();

            Helpers.AddProperty(props, null, new TextProperty("Test"));

            Assert.AreEqual(0, props.Count);
        }

        [Test]
        public void Ensure_Add_Property_No_Value()
        {
            Dictionary<string, IProperty> props = new Dictionary<string, IProperty>();

            Helpers.AddProperty(props, "Test", null);

            Assert.AreEqual(0, props.Count);
        }

        [Test]
        public void Ensure_Add_Property()
        {
            Dictionary<string, IProperty> props = new Dictionary<string, IProperty>();

            Helpers.AddProperty(props, "Test", new TextProperty("Value"));

            Assert.AreEqual(1, props.Count);
            Assert.IsTrue(props.ContainsKey("Test"));
            Assert.AreEqual("Value", props["Test"].ToString());
        }

        [Test]
        public void Ensure_Add_Text_Property()
        {
            Dictionary<string, IProperty> props = new Dictionary<string, IProperty>();

            Helpers.AddProperty(props, "Test", "Value", false);

            Assert.AreEqual(1, props.Count);
            Assert.IsTrue(props.ContainsKey("Test"));
            Assert.IsInstanceOf<TextProperty>(props["Test"]);
            Assert.AreEqual("Value", props["Test"].ToString());
        }

        [Test]
        public void Ensure_Add_Int_Property()
        {
            Dictionary<string, IProperty> props = new Dictionary<string, IProperty>();

            Helpers.AddProperty(props, "Test", (int) -5, false);

            Assert.AreEqual(1, props.Count);
            Assert.IsTrue(props.ContainsKey("Test"));
            Assert.IsInstanceOf<NumericProperty<int>>(props["Test"]);
            Assert.AreEqual("-5", props["Test"].ToString());
        }

        [Test]
        public void Ensure_Add_Uint_Property()
        {
            Dictionary<string, IProperty> props = new Dictionary<string, IProperty>();

            Helpers.AddProperty(props, "Test", (uint) 5, false);

            Assert.AreEqual(1, props.Count);
            Assert.IsTrue(props.ContainsKey("Test"));
            Assert.IsInstanceOf<NumericProperty<uint>>(props["Test"]);
            Assert.AreEqual("5", props["Test"].ToString());
        }

        [Test]
        public void Ensure_Add_Boolean_Property()
        {
            Dictionary<string, IProperty> props = new Dictionary<string, IProperty>();

            Helpers.AddProperty(props, "Test", true, false);

            Assert.AreEqual(1, props.Count);
            Assert.IsTrue(props.ContainsKey("Test"));
            Assert.IsInstanceOf<BooleanProperty>(props["Test"]);
            Assert.AreEqual("True", props["Test"].ToString());
        }
    }
}