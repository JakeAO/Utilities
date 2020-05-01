using System.Collections.Generic;
using Context;
using LookupTree.Nodes;
using LookupTree.Payloads;

namespace LookupTree
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