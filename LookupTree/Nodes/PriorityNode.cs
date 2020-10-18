using System.Collections.Generic;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.LookupTree.Payloads;

namespace SadPumpkin.Util.LookupTree.Nodes
{
    public class PriorityNode<T> : INode<T> where T : class, IPayload
    {
        public readonly List<INode<T>> Children = new List<INode<T>>(10);

        public T GetPayload(IContext bb)
        {
            foreach (var childNode in Children)
            {
                T payload = childNode?.GetPayload(bb);
                if (payload != null)
                    return payload;
            }
            return default;
        }

        public IEnumerable<INode<T>> EnumerateNodes()
        {
            yield return this;
            foreach (var childNode in Children)
            {
                foreach (var node in childNode.EnumerateNodes())
                {
                    yield return node;
                }
            }
        }
    }
}