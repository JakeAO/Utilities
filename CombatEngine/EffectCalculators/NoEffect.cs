using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.EffectCalculators
{
    public class NoEffect : IEffectCalc
    {
        public static readonly NoEffect Instance = new NoEffect();
        
        public void Apply(IInitiativeActor sourceEntity, IReadOnlyCollection<ICharacterActor> targetCharacters)
        {
            // Intentionally left blank
        }
    }
}