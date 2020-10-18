using SadPumpkin.Util.CombatEngine.EquipMap;
using SadPumpkin.Util.CombatEngine.Item.Armors;
using SadPumpkin.Util.CombatEngine.Item.Weapons;

namespace SadPumpkin.Util.CombatEngine.CharacterClasses
{
    public interface IPlayerClass : ICharacterClass
    {
        WeaponType WeaponProficiency { get; }
        ArmorType ArmorProficiency { get; }
        IEquipMapBuilder StartingEquipment { get; }
    }
}