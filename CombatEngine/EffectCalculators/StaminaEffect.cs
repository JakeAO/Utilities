using System;
using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.StatMap;

namespace SadPumpkin.Util.CombatEngine.EffectCalculators
{
    public class StaminaEffect : IEffectCalc
    {
        public Func<ICharacterActor, int> StaminaCalculation { get; }
        
        public string Description { get; }

        public StaminaEffect(Func<ICharacterActor, int> staminaCalculation, string description)
        {
            StaminaCalculation = staminaCalculation;
            Description = description;
        }

        public void Apply(IInitiativeActor sourceEntity, IReadOnlyCollection<ICharacterActor> targetCharacters)
        {
            if (sourceEntity is ICharacterActor sourceCharacter)
            {
                int stamina = StaminaCalculation(sourceCharacter);
                foreach (ICharacterActor targetCharacter in targetCharacters)
                {
                    targetCharacter.Stats.ModifyStat(StatType.STA, stamina);
                }
            }
        }
    }
}