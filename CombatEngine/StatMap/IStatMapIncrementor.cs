using System;

namespace SadPumpkin.Util.CombatEngine.StatMap
{
    public interface IStatMapIncrementor
    {
        IStatMap Increment(IStatMap statMap, Random random);
    }
}