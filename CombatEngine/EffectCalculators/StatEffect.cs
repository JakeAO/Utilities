using System;
using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.StatMap;

namespace SadPumpkin.Util.CombatEngine.EffectCalculators
{
    public class StatEffect : IEffectCalc
    {
        private static readonly Random RANDOM = new Random();

        public StatType Stat { get; }
        public Func<ICharacterActor, uint> MinCalculation { get; }
        public Func<ICharacterActor, uint> MaxCalculation { get; }

        public string Description { get; }

        public StatEffect(StatType stat, Func<ICharacterActor, uint> calculation, string description)
        {
            Stat = stat;
            MinCalculation = MaxCalculation = calculation;
            Description = description;
        }

        public StatEffect(StatType stat, Func<ICharacterActor, uint> minCalculation, Func<ICharacterActor, uint> maxCalculation, string description)
        {
            Stat = stat;
            MinCalculation = minCalculation;
            MaxCalculation = maxCalculation;
            Description = description;
        }

        public void Apply(IInitiativeActor sourceEntity, IReadOnlyCollection<ICharacterActor> targetCharacters)
        {
            if (sourceEntity is ICharacterActor sourceCharacter)
            {
                int effect = (int) -GetRawAmount(sourceCharacter);
                foreach (ICharacterActor targetCharacter in targetCharacters)
                {
                    targetCharacter.Stats.ModifyStat(Stat, effect);
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