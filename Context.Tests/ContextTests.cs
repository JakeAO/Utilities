using System.Collections;
using NUnit.Framework;

namespace SadPumpkin.Util.Context.Tests
{
    [TestFixture]
    public class ContextTests
    {
        private class IListProvider : IValueProvider<IList>
        {
            private readonly IList _theList;

            public IListProvider(IList theList)
            {
                _theList = theList;
            }

            public IList Get() => _theList;
        }

        [Test]
        public void Get_Returns_Null_When_Empty()
        {
            IContext context = new Context();

            IList result = context.Get<IList>();

            Assert.IsNull(result);
        }

        [Test]
        public void Get_Returns_Same_Object()
        {
            IList newList = new ArrayList();
            IContext context = new Context();
            context.SetValue(newList);

            IList result = context.Get<IList>();

            Assert.AreSame(newList, result);
        }

        [Test]
        public void Get_Returns_Same_Object_From_Base()
        {
            IList newList = new ArrayList();
            IContext baseContext = new Context();
            baseContext.SetValue(newList);

            IContext context = new Context(baseContext);

            IList result = context.Get<IList>();

            Assert.AreSame(newList, result);
        }

        [Test]
        public void Get_Returns_Value_From_Provider()
        {
            IList newList = new ArrayList();
            IListProvider provider = new IListProvider(newList);
            IContext context = new Context();
            context.SetProvider(provider);

            Assert.AreSame(newList, provider.Get());

            IList result = context.Get<IList>();

            Assert.AreSame(newList, result);
        }

        [Test]
        public void Clear_Removes_All_Values()
        {
            IList newList = new ArrayList();
            IContext context = new Context();
            context.SetValue(newList);

            context.Clear();
            IList result = context.Get<IList>();

            Assert.IsNull(result);
        }

        [Test]
        public void Clear_Removes_All_Providers()
        {
            IList newList = new ArrayList();
            IListProvider provider = new IListProvider(newList);
            IContext context = new Context();
            context.SetValue(newList);
            context.SetProvider(provider);

            Assert.IsNotNull(context.Get<IList>());

            context.Clear(true);

            Assert.IsNull(context.Get<IList>());
        }

        [Test]
        public void Clear_Type_Removes_All_Values_Of_Type()
        {
            ArrayList array = new ArrayList();
            IList list = array;
            IContext context = new Context();
            context.SetValue(array);
            context.SetValue(list);

            context.Clear<IList>();
            IList listResult = context.Get<IList>();
            ArrayList arrayResult = context.Get<ArrayList>();

            Assert.IsNull(listResult);
            Assert.IsNotNull(arrayResult);
        }

        [Test]
        public void Clear_Type_Removes_All_Values_Of_Type_And_Provider()
        {
            ArrayList array = new ArrayList();
            IList list = array;
            IListProvider provider = new IListProvider(list);
            IContext context = new Context();
            context.SetValue(array);
            context.SetValue(list);
            context.SetProvider(provider);

            Assert.IsNotNull(context.Get<IList>());

            context.Clear<IList>(true);

            Assert.IsNull(context.Get<IList>());
            Assert.IsNotNull(context.Get<ArrayList>());
        }
    }
}