using SadPumpkin.Util.CombatEngine.Abilities;

namespace SadPumpkin.Util.CombatEngine.Item.Weapons
{
    public interface IWeapon : IItem
    {
        WeaponType WeaponType { get; }
        IAbility AttackAbility { get; }
    }
}