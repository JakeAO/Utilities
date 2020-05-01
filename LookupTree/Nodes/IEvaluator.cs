using Context;

namespace LookupTree.Nodes
{
    public interface IEvaluator
    {
        bool Execute(IContext bb);
    }
}