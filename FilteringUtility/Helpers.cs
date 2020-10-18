using System;
using System.Collections.Generic;
using System.Linq;
using SadPumpkin.Util.FilteringUtility.Property;

namespace SadPumpkin.Util.FilteringUtility
{
    public static class Helpers
    {
        public static string GetListText(IEnumerable<string> list) => string.Join(" ", list);

        public static string GetEnumListText<T>(IEnumerable<T> enumList) where T : Enum => GetListText(enumList.Select(x => Enum.GetName(typeof(T), x)));

        public static void AddProperty(Dictionary<string, IProperty> props, string key, IProperty value)
        {
            if (string.IsNullOrWhiteSpace(key) || value == null)
                return;

            props[key] = value;
        }

        public static void AddProperty(Dictionary<string, IProperty> props, string key, string value, bool filterable) => AddProperty(props, key, new TextProperty(value, filterable));
        public static void AddProperty(Dictionary<string, IProperty> props, string key, int value, bool filterable) => AddProperty(props, key, new NumericProperty<int>(value, filterable));
        public static void AddProperty(Dictionary<string, IProperty> props, string key, uint value, bool filterable) => AddProperty(props, key, new NumericProperty<uint>(value, filterable));
        public static void AddProperty(Dictionary<string, IProperty> props, string key, bool value, bool filterable) => AddProperty(props, key, new BooleanProperty(value, filterable));
    }
}