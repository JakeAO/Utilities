using System.Collections.Generic;

namespace LookupTree.Payloads
{
    public interface IPayload
    {
        IEnumerable<string> GetFilePaths();
    }
}