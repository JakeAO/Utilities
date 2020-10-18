using System.Collections.Generic;

namespace SadPumpkin.Util.LookupTree.Payloads
{
    public interface IPayload
    {
        IEnumerable<string> GetFilePaths();
    }
}