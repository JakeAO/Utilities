using System.Collections.Generic;
using Context;
using LookupTree.Payloads;

namespace LookupTree.Nodes
{
    public class ConditionNode<T> : INode<T> where T : class, IPayload
    {
        public IEvaluator Evaluator;
        public INode<T> Child;

        public T GetPayload(IContext bb)
        {
            if (Child == null)
                return default;

            if (Evaluator == null)
                return default;

            if (!Evaluator.Execute(bb))
                return default;

            return Child.GetPayload(bb);
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