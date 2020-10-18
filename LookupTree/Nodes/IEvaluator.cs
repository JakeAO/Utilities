using SadPumpkin.Util.Context;

namespace SadPumpkin.Util.LookupTree.Nodes
{
    public interface IEvaluator
    {
        bool Execute(IContext bb);
    }
}