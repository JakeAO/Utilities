using System;
using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.StatMap;

namespace SadPumpkin.Util.CombatEngine.EffectCalculators
{
    public class RawDamageEffect : IEffectCalc
    {
        public Func<ICharacterActor, uint> DamageCalculation { get; }

        public RawDamageEffect(Func<ICharacterActor, uint> damageCalculation)
        {
            DamageCalculation = damageCalculation;
        }

        public void Apply(IInitiativeActor sourceEntity, IReadOnlyCollection<ICharacterActor> targetCharacters)
        {
            if (sourceEntity is ICharacterActor sourceCharacter)
            {
                int damage = (int) -DamageCalculation(sourceCharacter);
                foreach (ICharacterActor targetCharacter in targetCharacters)
                {
                    targetCharacter.Stats.ModifyStat(StatType.HP, damage);
                }
            }
        }
    }
}