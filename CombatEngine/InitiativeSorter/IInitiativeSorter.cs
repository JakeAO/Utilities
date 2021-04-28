using System.Collections.Generic;

namespace SadPumpkin.Util.CombatEngine.InitiativeSorter
{
    public interface IInitiativeSorter
    {
        IInitiativePair GetNext(IEnumerable<IInitiativePair> initiativePairs);
        IEnumerable<IInitiativePair> PredictNext(IEnumerable<IInitiativePair> initiativePairs, uint count);
    }
}