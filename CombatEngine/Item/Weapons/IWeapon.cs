namespace SadPumpkin.Util.CombatEngine.Item.Weapons
{
    public interface IWeapon : IItem
    {
        WeaponType WeaponType { get; }
    }
}