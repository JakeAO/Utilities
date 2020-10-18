using System.Collections.Generic;
using SadPumpkin.Util.Context;

namespace SadPumpkin.Util.LookupTree.Nodes
{
    public interface IIndirector
    {
        IEnumerable<IContext> Execute(IContext bb);
        void SetCache(IContext bb);
        void ClearCache(IContext bb);
    }
}