using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.EffectCalculators
{
    public interface IEffectCalc
    {
        void Apply(IInitiativeActor sourceEntity, IReadOnlyCollection<ICharacterActor> targetCharacters);
    }
}