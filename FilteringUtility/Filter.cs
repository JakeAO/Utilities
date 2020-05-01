using System;
using System.Collections.Generic;
using System.Linq;

namespace FilteringUtility
{
    public readonly struct Filter
    {
        public readonly string FullFilterText;
        public readonly IReadOnlyList<string> GeneralFilters;
        public readonly IReadOnlyDictionary<string, HashSet<string>> PropertyFilters;

        public Filter(string fullFilterText, char filterSplitter = '+', char propertySplitter = '=', char valueSplitter = '&')
        {
            string filterSplitterString = $" {filterSplitter} ";
            string propertySplitterString = $"{propertySplitter}";
            string valueSplitterString = $"{valueSplitter}";

            string[] filterSplitterArray = {filterSplitterString};
            string[] propertySplitterArray = {propertySplitterString};
            string[] valueSplitterArray = {valueSplitterString};

            if (fullFilterText is null)
                fullFilterText = string.Empty;

            List<string> generalFilters = new List<string>(10);
            Dictionary<string, HashSet<string>> propertyFilters = new Dictionary<string, HashSet<string>>(10, StringComparer.InvariantCultureIgnoreCase);

            string[] splitFilter = fullFilterText.Split(filterSplitterArray, StringSplitOptions.RemoveEmptyEntries);
            foreach (string rawFilterEntry in splitFilter)
            {
                string[] filterKeyValue = rawFilterEntry.Split(propertySplitterArray, StringSplitOptions.None);
                switch (filterKeyValue.Length)
                {
                    case 1:
                    {
                        string value1 = filterKeyValue[0].Trim();
                        if (string.IsNullOrWhiteSpace(value1))
                            break;

                        generalFilters.Add(value1);
                        break;
                    }
                    case 2:
                    {
                        string value1 = filterKeyValue[0].Trim();
                        if (string.IsNullOrWhiteSpace(value1))
                            break;

                        if (!propertyFilters.TryGetValue(value1, out HashSet<string> hashSet))
                            propertyFilters[value1] = hashSet = new HashSet<string>();

                        string[] values = filterKeyValue[1].Split(valueSplitterArray, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string value in values)
                        {
                            string value2 = value.Trim();
                            if (string.IsNullOrWhiteSpace(value2))
                                continue;

                            hashSet.Add(value2);
                        }

                        break;
                    }
                }
            }

            GeneralFilters = generalFilters;
            PropertyFilters = propertyFilters;

            FullFilterText = string.Empty;
            if (generalFilters.Count > 0)
            {
                string generalFilterString = string.Join(filterSplitterString, generalFilters);
                FullFilterText += generalFilterString;
            }

            if (propertyFilters.Count > 0)
            {
                if (generalFilters.Count > 0)
                {
                    FullFilterText += filterSplitterString;
                }

                FullFilterText += string.Join(filterSplitterString,
                    propertyFilters
                        .Select(x => $"{x.Key}{propertySplitterString}{string.Join(valueSplitterString, x.Value)}"));
            }
        }
    }
}