using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Abilities;
using SadPumpkin.Util.CombatEngine.EquipMap;
using SadPumpkin.Util.CombatEngine.Item.Armors;
using SadPumpkin.Util.CombatEngine.Item.Weapons;
using SadPumpkin.Util.CombatEngine.StatMap;

namespace SadPumpkin.Util.CombatEngine.CharacterClasses
{
    public class PlayerClass : CharacterClass, IPlayerClass
    {
        public WeaponType WeaponProficiency { get; }
        public ArmorType ArmorProficiency { get; }
        public IEquipMapBuilder StartingEquipment { get; }

        public PlayerClass(
            uint id,
            string name,
            string desc,
            IReadOnlyDictionary<DamageType, float> intrinsicDamageModification,
            IStatMapBuilder startingStats,
            IStatMapIncrementor levelUpStats,
            IReadOnlyDictionary<uint, IReadOnlyCollection<IAbility>> abilitiesPerLevel,
            WeaponType weaponProficiency,
            ArmorType armorProficiency,
            IEquipMapBuilder startingEquipment)
            : base(
                id,
                name,
                desc,
                intrinsicDamageModification,
                startingStats,
                levelUpStats,
                abilitiesPerLevel)
        {
            WeaponProficiency = weaponProficiency;
            ArmorProficiency = armorProficiency;
            StartingEquipment = startingEquipment;
        }
    }
}