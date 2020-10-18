using System.Collections.Generic;
using SadPumpkin.Util.Context;
using SadPumpkin.Util.LookupTree.Payloads;

namespace SadPumpkin.Util.LookupTree.Nodes
{
    public class IndirectionNode<T> : INode<T> where T : class, IPayload
    {
        public INode<T> Child;
        public IIndirector Indirector;

        public T GetPayload(IContext bb)
        {
            if (Child == null)
                return default;

            if (Indirector == null)
                return default;

            Indirector.SetCache(bb);
            T payload = default;
            
            foreach (IContext modifiedBb in Indirector.Execute(bb))
            {
                payload = Child.GetPayload(modifiedBb);
                if (payload != default)
                    break;
            }
            
            Indirector.ClearCache(bb);

            return payload;
        }

        public IEnumerable<INode<T>> EnumerateNodes()
        {
            yield return this;
            if (Child != null)
            {
                yield return Child;
            }
        }
    }
}