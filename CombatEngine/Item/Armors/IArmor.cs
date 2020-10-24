using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Item.Weapons;

namespace SadPumpkin.Util.CombatEngine.Item.Armors
{
    public interface IArmor : IItem
    {
        ArmorType ArmorType { get; }
        IReadOnlyDictionary<DamageType, float> DamageModifiers { get; }
        float GetReducedDamage(float damageAmount, DamageType damageType);
    }
}