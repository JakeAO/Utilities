using System;
using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Abilities;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.CharacterClasses;
using SadPumpkin.Util.CombatEngine.Item.Weapons;
using SadPumpkin.Util.CombatEngine.StatMap;

namespace SadPumpkin.Util.CombatEngine.Actor
{
    public class Character : ICharacterActor
    {
        public uint Id { get; set; }
        public uint Party { get; set; }
        public string Name { get; set; }
        public ICharacterClass Class { get; set; }
        public IStatMap Stats { get; set; }
        
        public bool IsAlive() => Stats[StatType.HP] > 0u;
        public bool CanTarget() => IsAlive();
        public float GetInitiative() => 10f + 5f * (Stats[StatType.DEX] / 80f);

        public Character()
            : this(0, 0, string.Empty, NullClass.Instance, new StatMap.StatMap())
        {
        }

        public Character(
            uint id,
            uint party,
            string name,
            ICharacterClass characterClass,
            IStatMap stats)
        {
            Id = id;
            Party = party;
            Name = name;
            Class = characterClass;
            Stats = stats;
        }

        public virtual float GetReducedDamage(float damageAmount, DamageType damageType)
        {
            if (Class.IntrinsicDamageModification != null && Class.IntrinsicDamageModification.Count > 0)
            {
                float damage = damageAmount;
                if (Class.IntrinsicDamageModification.TryGetValue(damageType, out float modifier))
                {
                    damage *= modifier;
                }
                else
                {
                    foreach (int enumValue in Enum.GetValues(typeof(DamageType)))
                    {
                        DamageType enumType = (DamageType) enumValue;
                        if ((enumValue & (int) damageType) == enumValue &&
                            Class.IntrinsicDamageModification.TryGetValue(enumType, out modifier))
                        {
                            damage *= modifier;
                        }
                    }
                }

                return damage;
            }

            return damageAmount;
        }

        public virtual IReadOnlyCollection<IAction> GetAllActions(IReadOnlyCollection<ITargetableActor> possibleTargets)
        {
            List<IAction> actions = new List<IAction>(10);

            foreach (IAbility ability in Class.GetAllAbilities(Stats[StatType.LVL]))
            {
                actions.AddRange(ActionUtil.GetActionsForAbility(ability, this, possibleTargets));
            }

            actions.Add(new WaitAction(this));

            return actions;
        }

        public virtual IInitiativeActor Copy()
        {
            return new Character(
                Id,
                Party,
                Name,
                Class,
                Stats.Copy());
        }
    }
}