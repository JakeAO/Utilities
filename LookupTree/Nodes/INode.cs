using System.Collections.Generic;
using Context;
using LookupTree.Payloads;

namespace LookupTree.Nodes
{
    public interface INode<T> where T : class, IPayload
    {
        T GetPayload(IContext bb);

        IEnumerable<INode<T>> EnumerateNodes();
    }
}