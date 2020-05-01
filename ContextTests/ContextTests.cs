using System.Collections;
using NUnit.Framework;

namespace Context.Tests
{
    [TestFixture]
    public class ContextTests
    {
        [Test]
        public void Get_Returns_Null_When_Empty()
        {
            IContext context = new Context();

            IList result = context.Get<IList>();

            Assert.IsNull(result);
        }

        [Test]
        public void Get_Returns_Null_With_Key_When_Empty()
        {
            IContext context = new Context();

            IList result = context.Get<IList>("testKey");

            Assert.IsNull(result);
        }

        [Test]
        public void Get_Returns_Same_Object()
        {
            IList newList = new ArrayList();
            IContext context = new Context();
            context.Set(newList);

            IList result = context.Get<IList>();

            Assert.AreSame(newList, result);
        }

        [Test]
        public void Get_Returns_Same_Object_With_Key()
        {
            IList newList = new ArrayList();
            IContext context = new Context();
            context.Set(newList, "testKey");

            IList result = context.Get<IList>("testKey");

            Assert.AreSame(newList, result);
        }

        [Test]
        public void Get_Returns_Same_Object_From_Base()
        {
            IList newList = new ArrayList();
            IContext baseContext = new Context();
            baseContext.Set(newList);

            IContext context = new Context(baseContext);

            IList result = context.Get<IList>();

            Assert.AreSame(newList, result);
        }

        [Test]
        public void Get_Returns_Same_Object_From_Base_With_Key()
        {
            IList newList = new ArrayList();
            IContext baseContext = new Context();
            baseContext.Set(newList, "testKey");

            IContext context = new Context(baseContext);

            IList result = context.Get<IList>("testKey");

            Assert.AreSame(newList, result);
        }

        [Test]
        public void Clear_Removes_All_Values()
        {
            IList newList = new ArrayList();
            IContext context = new Context();
            context.Set(newList);

            context.Clear();
            IList result = context.Get<IList>();

            Assert.IsNull(result);
        }

        [Test]
        public void Clear_Type_Removes_All_Values_Of_Type()
        {
            ArrayList array = new ArrayList();
            IList list = array;
            IContext context = new Context();
            context.Set(array);
            context.Set(list);

            context.Clear<IList>();
            IList listResult = context.Get<IList>();
            ArrayList arrayResult = context.Get<ArrayList>();

            Assert.IsNull(listResult);
            Assert.IsNotNull(arrayResult);
        }

        [Test]
        public void Clear_Key_Removes_Value_At_Key()
        {
            IList list = new ArrayList();
            IContext context = new Context();
            context.Set(list, "testKey1");
            context.Set(list, "testKey2");

            context.Clear<IList>("testKey1");
            IList listResult1 = context.Get<IList>("testKey1");
            IList listResult2 = context.Get<IList>("testKey2");

            Assert.IsNull(listResult1);
            Assert.NotNull(listResult2);
        }
    }
}