using System.Collections;
using System.Collections.Generic;
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
        
        private class ListProvider : IValueProvider<List<int>>
        {
            private readonly List<int> _theList;

            public ListProvider(List<int> theList)
            {
                _theList = theList;
            }

            public List<int> Get() => _theList;
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
            IMutableContext context = new MutableContext();
            context.SetValue(newList);

            IList result = context.Get<IList>();

            Assert.AreSame(newList, result);
        }

        [Test]
        public void Get_Returns_Same_Object_From_Base()
        {
            IList newList = new ArrayList();
            IMutableContext baseContext = new MutableContext();
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
            IMutableContext context = new MutableContext();
            context.SetProvider(provider);

            Assert.AreSame(newList, provider.Get());

            IList result = context.Get<IList>();

            Assert.AreSame(newList, result);
        }

        [Test]
        public void Get_Returns_List_When_IList_Is_Requested()
        {
            IList newList1 = new ArrayList();
            List<int> newList2 = new List<int>();
            IMutableContext context = new MutableContext();
            context.SetValue(newList1);
            context.SetValue(newList2);

            Assert.AreSame(newList1, context.Get<IList>());
            Assert.AreSame(newList2, context.Get<List<int>>());

            context.Clear<IList>();

            Assert.AreSame(newList2, context.Get<IList>(false));
            Assert.AreSame(newList2, context.Get<List<int>>(false));
        }

        [Test]
        public void Get_Returns_List_When_IList_Is_Requested_From_Provider()
        {
            IList newList1 = new ArrayList();
            List<int> newList2 = new List<int>();
            IListProvider provider1 = new IListProvider(newList1);
            ListProvider provider2 = new ListProvider(newList2);
            IMutableContext context = new MutableContext();
            context.SetProvider(provider1);
            context.SetProvider(provider2);
            
            Assert.AreSame(newList1, context.Get<IList>());
            Assert.AreSame(newList2, context.Get<List<int>>());

            context.Clear<IList>(true);

            Assert.AreSame(newList2, context.Get<IList>(false));
            Assert.AreSame(newList2, context.Get<List<int>>(false));
        }

        [Test]
        public void Clear_Removes_All_Values()
        {
            IList newList = new ArrayList();
            IMutableContext context = new MutableContext();
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
            IMutableContext context = new MutableContext();
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
            IMutableContext context = new MutableContext();
            context.SetValue(array);
            context.SetValue(list);

            context.Clear<ArrayList>();
            IList listResult = context.Get<IList>();
            ArrayList arrayResult = context.Get<ArrayList>();

            Assert.IsNotNull(listResult);
            Assert.IsNull(arrayResult);
        }

        [Test]
        public void Clear_Type_Removes_All_Values_Of_Type_And_Provider()
        {
            IList list = new ArrayList();
            IListProvider provider = new IListProvider(list);
            IMutableContext context = new MutableContext();
            context.SetValue(list);
            context.SetProvider(provider);

            Assert.IsNotNull(context.Get<IList>());

            context.Clear<IList>(true);

            Assert.IsNull(context.Get<IList>());
        }
    }
}