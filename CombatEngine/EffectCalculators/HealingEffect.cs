using System;
using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.StatMap;

namespace SadPumpkin.Util.CombatEngine.EffectCalculators
{
    public class HealingEffect : IEffectCalc
    {
        private static readonly Random RANDOM = new Random();

        public Func<ICharacterActor, uint> MinHealingCalculation { get; }
        public Func<ICharacterActor, uint> MaxHealingCalculation { get; }

        public HealingEffect(Func<ICharacterActor, uint> healingCalculation)
        {
            MinHealingCalculation = MaxHealingCalculation = healingCalculation;
        }

        public HealingEffect(Func<ICharacterActor, uint> minHealingCalculation, Func<ICharacterActor, uint> maxHealingCalculation)
        {
            MinHealingCalculation = minHealingCalculation;
            MaxHealingCalculation = maxHealingCalculation;
        }

        public void Apply(IInitiativeActor sourceEntity, IReadOnlyCollection<ICharacterActor> targetCharacters)
        {
            if (sourceEntity is ICharacterActor sourceCharacter)
            {
                int healing = (int) GetRawHealing(sourceCharacter);
                foreach (ICharacterActor targetCharacter in targetCharacters)
                {
                    targetCharacter.Stats.ModifyStat(StatType.HP, healing);
                }
            }
        }

        private uint GetRawHealing(ICharacterActor sourceCharacter)
        {
            if (MinHealingCalculation == MaxHealingCalculation)
                return MinHealingCalculation(sourceCharacter);

            uint min = MinHealingCalculation(sourceCharacter);
            uint max = MaxHealingCalculation(sourceCharacter) + 1; // Random.Next max is exclusive, so add 1.
            return (uint) RANDOM.Next((int) min, (int) max);
        }
    }
}