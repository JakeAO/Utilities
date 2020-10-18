using SadPumpkin.Util.CombatEngine.Item.Weapons;

namespace SadPumpkin.Util.CombatEngine.Item.Armors
{
    public interface IArmor : IItem
    {
        ArmorType ArmorType { get; }

        float GetReducedDamage(float damageAmount, DamageType damageType);
    }
}