using System;
using System.Linq;
using System.Collections.Generic;
using FilteringUtility.Property;

namespace FilteringUtility
{
    public readonly struct Entry<U>
    {
        public readonly U TrackingKey;
        public readonly string CombinedString;
        public readonly IReadOnlyDictionary<string, IProperty> Properties;

        public Entry(U trackingKey, IReadOnlyDictionary<string, IProperty> propertyText)
        {
            TrackingKey = trackingKey;

            // Convert input dictionary to an IgnoreCase dictionary so we can filter on lowercase values.
            var propertyDictionary = new Dictionary<string, IProperty>(10, StringComparer.InvariantCultureIgnoreCase);
            foreach (var propertyKvp in propertyText)
            {
                propertyDictionary[propertyKvp.Key] = propertyKvp.Value;
            }

            Properties = propertyDictionary;
            CombinedString = string.Join(" ", Properties.Values.Select(x => x.ToString()));
        }
    }
}