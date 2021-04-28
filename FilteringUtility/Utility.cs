using System;
using System.Collections.Generic;
using System.Linq;
using SadPumpkin.Util.FilteringUtility.Property;

namespace SadPumpkin.Util.FilteringUtility
{
    public class Utility<T, U>
    {
        public readonly IReadOnlyList<Entry<U>> AllEntries = null;
        public readonly IReadOnlyList<KeyValuePair<string, IReadOnlyList<string>>> AllKnownEntryProperties = null;

        public Filter CurrentFilter { get; private set; }

        public IReadOnlyList<U> FilteredValues => _currentFilteredValues;

        private readonly List<U> _currentFilteredValues = new List<U>(100);

        public Utility(IReadOnlyCollection<T> collection, Func<T, Entry<U>> entryGenerator)
        {
            AllEntries = collection.Select(entryGenerator).ToList();
            _currentFilteredValues.AddRange(AllEntries.Select(x => x.TrackingKey));

            Dictionary<string, List<string>> knownProperties = new Dictionary<string, List<string>>();
            foreach (var entry in AllEntries)
            {
                foreach (var propertyKvp in entry.Properties)
                {
                    if (!knownProperties.TryGetValue(propertyKvp.Key, out List<string> knownValues))
                        knownProperties[propertyKvp.Key] = knownValues = new List<string>();

                    if (!propertyKvp.Value.CanQuickFilter)
                        continue;

                    string[] splitPropertyValue = propertyKvp.Value.ToString().Split(' ');
                    foreach (string propertyValue in splitPropertyValue)
                        if (!knownValues.Contains(propertyValue))
                            knownValues.Add(propertyValue);
                }
            }

            var finalProps = new List<KeyValuePair<string, IReadOnlyList<string>>>();
            foreach (var knownPropKvp in knownProperties.OrderBy(x => x.Key))
            {
                List<string> valueList = knownPropKvp.Value;
                valueList.Sort((lhs, rhs) =>
                {
                    bool lhsInt = int.TryParse(lhs, out int lhsVal);
                    bool rhsInt = int.TryParse(rhs, out int rhsVal);

                    int result = lhsInt.CompareTo(rhsInt);
                    if (result != 0)
                        return result;

                    if (lhsInt && rhsInt)
                        return lhsVal.CompareTo(rhsVal);

                    return string.Compare(lhs, rhs, StringComparison.InvariantCultureIgnoreCase);
                });
                finalProps.Add(new KeyValuePair<string, IReadOnlyList<string>>(knownPropKvp.Key, valueList));
            }

            AllKnownEntryProperties = finalProps;
        }

        public void UpdateFilter(string newFilterText)
        {
            CurrentFilter = new Filter(newFilterText);

            UpdateFilteredValues();
        }

        private void UpdateFilteredValues()
        {
            _currentFilteredValues.Clear();

            if (CurrentFilter.GeneralFilters.Count == 0 &&
                CurrentFilter.PropertyFilters.Count == 0)
            {
                _currentFilteredValues.AddRange(AllEntries.Select(x => x.TrackingKey));
                return;
            }

            foreach (var entry in AllEntries)
            {
                bool failOut = false;
                foreach (var propertyFilter in CurrentFilter.PropertyFilters)
                {
                    if (!entry.Properties.TryGetValue(propertyFilter.Key, out IProperty propertyValue))
                    {
                        failOut = true;
                        break;
                    }

                    foreach (string propertyFilterValue in propertyFilter.Value)
                    {
                        if (propertyValue.ToString().Contains(propertyFilterValue))//, StringComparison.InvariantCultureIgnoreCase))
                        {
                            failOut = true;
                            break;
                        }
                    }

                    if (failOut)
                        break;
                }

                if (failOut)
                    continue;

                foreach (var generalFilter in CurrentFilter.GeneralFilters)
                {
                    if (entry.CombinedString.Contains(generalFilter))//, StringComparison.InvariantCultureIgnoreCase))
                    {
                        failOut = true;
                        break;
                    }
                }

                if (failOut)
                    continue;

                _currentFilteredValues.Add(entry.TrackingKey);
            }
        }
    }
}