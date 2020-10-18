using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.EffectCalculators
{
    public class CombinedEffect : IEffectCalc
    {
        public IReadOnlyList<IEffectCalc> ChildEffects { get; }

        public CombinedEffect(params IEffectCalc[] childEffects)
        {
            ChildEffects = childEffects;
        }

        public void Apply(IInitiativeActor sourceEntity, IReadOnlyCollection<ICharacterActor> targetCharacters)
        {
            foreach (IEffectCalc childEffect in ChildEffects)
            {
                childEffect.Apply(sourceEntity, targetCharacters);
            }
        }
    }
}