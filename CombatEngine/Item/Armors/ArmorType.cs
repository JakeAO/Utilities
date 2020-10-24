using System;

namespace SadPumpkin.Util.CombatEngine.Item.Armors
{
    [Flags]
    public enum ArmorType
    {
        Invalid = 0,
        
        Light = 1,
        Medium = 2,
        Heavy = 4
    }
}