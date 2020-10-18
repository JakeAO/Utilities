using System;

namespace SadPumpkin.Util.CombatEngine.EquipMap
{
    public interface IEquipMapBuilder
    {
        IEquipMap Generate(Random random);
    }
}