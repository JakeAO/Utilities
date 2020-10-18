using System.Collections.Generic;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.LookupTree.Payloads;

namespace SadPumpkin.Util.LookupTree.Nodes
{
    public interface INode<T> where T : class, IPayload
    {
        T GetPayload(IContext bb);

        IEnumerable<INode<T>> EnumerateNodes();
    }
}