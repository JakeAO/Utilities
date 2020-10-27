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
        public Func<ICharacterActor, uint> MinCalculation { get; }
        public Func<ICharacterActor, uint> MaxCalculation { get; }
        
        public string Description { get; }

        public DamageEffect(DamageType damageType, Func<ICharacterActor, uint> calculation, string description)
        {
            DamageType = damageType;
            MinCalculation = MaxCalculation = calculation;
            Description = description;
        }

        public DamageEffect(DamageType damageType, Func<ICharacterActor, uint> minCalculation, Func<ICharacterActor, uint> maxCalculation, string description)
        {
            DamageType = damageType;
            MinCalculation = minCalculation;
            MaxCalculation = maxCalculation;
            Description = description;
        }

        public void Apply(IInitiativeActor sourceEntity, IReadOnlyCollection<ICharacterActor> targetCharacters)
        {
            if (sourceEntity is ICharacterActor sourceCharacter)
            {
                uint damage = GetRawAmount(sourceCharacter);
                foreach (ICharacterActor targetCharacter in targetCharacters)
                {
                    float modifiedDamage = targetCharacter.GetReducedDamage(damage, DamageType);
                    targetCharacter.Stats.ModifyStat(StatType.HP, (int) -Math.Round(modifiedDamage));
                }
            }
        }

        private uint GetRawAmount(ICharacterActor sourceCharacter)
        {
            if (MinCalculation == MaxCalculation)
                return MinCalculation(sourceCharacter);

            uint min = MinCalculation(sourceCharacter);
            uint max = MaxCalculation(sourceCharacter) + 1; // Random.Next max is exclusive, so add 1.
            return (uint) RANDOM.Next((int) min, (int) max);
        }
    }
}