using System;
using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.Item.Weapons;
using SadPumpkin.Util.CombatEngine.StatMap;

namespace SadPumpkin.Util.CombatEngine.EffectCalculators
{
    public class DamageEffect : IEffectCalc
    {
        private static readonly Random RANDOM = new Random();

        public DamageType DamageType { get; }
        public Func<ICharacterActor, uint> MinDamageCalculation { get; }
        public Func<ICharacterActor, uint> MaxDamageCalculation { get; }

        public DamageEffect(DamageType damageType, Func<ICharacterActor, uint> damageCalculation)
        {
            DamageType = damageType;
            MinDamageCalculation = MaxDamageCalculation = damageCalculation;
        }

        public DamageEffect(DamageType damageType, Func<ICharacterActor, uint> minDamageCalculation, Func<ICharacterActor, uint> maxDamageCalculation)
        {
            DamageType = damageType;
            MinDamageCalculation = minDamageCalculation;
            MaxDamageCalculation = maxDamageCalculation;
        }

        public void Apply(IInitiativeActor sourceEntity, IReadOnlyCollection<ICharacterActor> targetCharacters)
        {
            if (sourceEntity is ICharacterActor sourceCharacter)
            {
                uint damage = GetRawDamage(sourceCharacter);
                foreach (ICharacterActor targetCharacter in targetCharacters)
                {
                    float modifiedDamage = targetCharacter.GetReducedDamage(damage, DamageType);
                    targetCharacter.Stats.ModifyStat(StatType.HP, (int) -Math.Round(modifiedDamage));
                }
            }
        }

        private uint GetRawDamage(ICharacterActor sourceCharacter)
        {
            if (MinDamageCalculation == MaxDamageCalculation)
                return MinDamageCalculation(sourceCharacter);

            uint min = MinDamageCalculation(sourceCharacter);
            uint max = MaxDamageCalculation(sourceCharacter) + 1; // Random.Next max is exclusive, so add 1.
            return (uint) RANDOM.Next((int) min, (int) max);
        }
    }
}