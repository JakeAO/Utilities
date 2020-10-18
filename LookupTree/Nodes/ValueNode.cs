using System.Collections.Generic;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.LookupTree.Payloads;

namespace SadPumpkin.Util.LookupTree.Nodes
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