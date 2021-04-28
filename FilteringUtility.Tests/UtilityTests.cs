using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;
using SadPumpkin.Util.FilteringUtility.Property;

namespace SadPumpkin.Util.FilteringUtility.Tests
{
    [TestFixture]
    public class UtilityTests
    {
        private static Entry<string> EntryGenerator(CultureInfo ci) =>
            new Entry<string>(ci.Name, new Dictionary<string, IProperty>()
            {
                {"Name", new TextProperty(ci.Name, true)},
                {"ISO", new TextProperty(ci.ThreeLetterISOLanguageName, true)},
                {"NativeName", new TextProperty(ci.NativeName, true)}
            });
        
        private Utility<CultureInfo, string> _utility = null;

        private IReadOnlyList<CultureInfo> _allCultures = null;
        private HashSet<string> _allCultureNames = null;
        private HashSet<string> _allIsoCodes = null;
        private HashSet<string> _allNativeNames = null;

        [SetUp]
        public void Setup()
        {
            _allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(x => x.Name.Equals(x.IetfLanguageTag)).ToList();
            _allCultureNames = _allCultures.Select(x => x.Name).ToHashSet();
            _allIsoCodes = _allCultures.Select(x => x.ThreeLetterISOLanguageName).ToHashSet();
            _allNativeNames = _allCultures.Select(x => x.NativeName).ToHashSet();

            _utility = new Utility<CultureInfo, string>(_allCultures, EntryGenerator);
        }

        [Test]
        public void Ensure_Entry_Count()
        {
            Assert.AreEqual(_allCultures.Count, _utility.AllEntries.Count);
        }

        [Test]
        public void Ensure_Entry_Accuracy()
        {
            Assert.IsTrue(_allCultureNames.SetEquals(_utility.AllEntries.Select(x => x.TrackingKey)));
        }
    }
}