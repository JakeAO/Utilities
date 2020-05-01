using System;
using Context;

namespace LookupTree.Nodes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EnumTypeAttribute : Attribute
    {
        public Type EnumType { get; }

        public EnumTypeAttribute(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (!typeof(Enum).IsAssignableFrom(type))
                throw new ArgumentException($"type {type.Name} is not an Enum");
            if (!type.IsEnum)
                throw new ArgumentException($"type {type.Name} is not an Enum");

            EnumType = type;
        }
    }
    
    public interface IExtractor<T>
    {
        bool Execute(IContext bb, out T value);
    }
}