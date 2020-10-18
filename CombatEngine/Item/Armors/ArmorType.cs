using System;

namespace SadPumpkin.Util.CombatEngine.Item.Armors
{
    [Flags]
    public enum ArmorType
    {
        Invalid = 1,
        
        Light = 2,
        Medium = 4,
        Heavy = 8
    }
}