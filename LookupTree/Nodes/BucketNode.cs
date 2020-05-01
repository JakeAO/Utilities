using System.Collections.Generic;
using Context;
using LookupTree.Payloads;

namespace LookupTree.Nodes
{
    public class BucketNode<T, U> : INode<T> where T : class, IPayload
    {
        public IExtractor<U> Extractor;
        public readonly Dictionary<U, INode<T>> Children = new Dictionary<U, INode<T>>(10);

        public T GetPayload(IContext bb)
        {
            if (Extractor == null)
                return default;

            if (!Extractor.Execute(bb, out U key))
                return default;

            if (!Children.TryGetValue(key, out INode<T> childNode) || childNode == null)
                return default;

            return childNode.GetPayload(bb);
        }

        public IEnumerable<INode<T>> EnumerateNodes()
        {
            yield return this;
            foreach (var childNode in Children.Values)
            {
                foreach (var node in childNode.EnumerateNodes())
                {
                    yield return node;
                }
            }
        }
    }
}