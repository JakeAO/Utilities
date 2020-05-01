using System.Collections.Generic;
using Context;
using LookupTree.Payloads;

namespace LookupTree.Nodes
{
    public class ValueNode<T> : INode<T> where T : class, IPayload
    {
        public T Payload;

        public T GetPayload(IContext bb)
        {
            return Payload;
        }

        public IEnumerable<INode<T>> EnumerateNodes()
        {
            yield return this;
        }
    }
}