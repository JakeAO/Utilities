using System.Collections.Generic;
using Context;

namespace LookupTree.Nodes
{
    public interface IIndirector
    {
        IEnumerable<IContext> Execute(IContext bb);
        void SetCache(IContext bb);
        void ClearCache(IContext bb);
    }
}