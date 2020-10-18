using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Abilities;
using SadPumpkin.Util.CombatEngine.Item.Weapons;
using SadPumpkin.Util.CombatEngine.StatMap;

namespace SadPumpkin.Util.CombatEngine.CharacterClasses
{
    public interface ICharacterClass : IIdTracked
    {
        string Name { get; }
        string Desc { get; }
        IReadOnlyDictionary<DamageType, float> IntrinsicDamageModification { get; }
        IStatMapBuilder StartingStats { get; }
        IStatMapIncrementor LevelUpStats { get; }
        IReadOnlyDictionary<uint, IReadOnlyCollection<IAbility>> AbilitiesPerLevel { get; }

        IReadOnlyCollection<IAbility> GetAllAbilities(uint level);
    }
}