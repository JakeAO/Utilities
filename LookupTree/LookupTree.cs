using System.Collections.Generic;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.LookupTree.Nodes;
using SadPumpkin.Util.LookupTree.Payloads;

namespace SadPumpkin.Util.LookupTree
{
    public class LookupTree<T> where T : class, IPayload
    {
        public INode<T> Root;
        
        public T GetPayload(IContext bb)
        {
            return Root?.GetPayload(bb);
        }
        
        public IEnumerable<INode<T>> EnumerateNodes()
        {
            if (Root != null)
            {
                foreach (var node in Root.EnumerateNodes())
                {
                    yield return node;
                }
            }
        }
    }
}