using System;

namespace SadPumpkin.Util.CombatEngine.StatMap
{
    public interface IStatMapBuilder
    {
        IStatMap Generate(Random random);
    }
}