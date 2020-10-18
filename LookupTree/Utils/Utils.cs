using System;
using SadPumpkin.Util.LookupTree.Payloads;

namespace SadPumpkin.Util.LookupTree.Utils
{
    public static class Utils
    {
        public static string GetPayloadTypeName<T>() where T : IPayload
        {
            return GetPayloadTypeName(typeof(T));
        }

        public static string GetPayloadTypeName(Type type)
        {
            if (type is null)
                return "INVALID";
            if (type.IsAbstract || type.IsInterface)
                return "INVALID";
            if (!typeof(IPayload).IsAssignableFrom(type))
                return "INVALID";

            const string PREAMBLE = "LookupTree.Payloads.";
            return type.FullName.Replace(PREAMBLE, string.Empty);
        }
    }
}