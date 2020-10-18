﻿using System;

namespace SadPumpkin.Util.CombatEngine.Item.Weapons
{
    [Flags]
    public enum DamageType
    {
        Invalid = 0,
        
        Normal = 1,
        Fire = 2,
        Water = 4,
        Wind = 8,
        Stone = 16,
        
        Dark = 32,
        Light = 64
    }
}