using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.TargetCalculators
{
    public interface ITargetCalc
    {
        bool CanTarget(ICharacterActor sourceCharacter, ICharacterActor targetCharacter);
        IReadOnlyCollection<IReadOnlyCollection<ICharacterActor>> GetTargetOptions(ICharacterActor sourceCharacter, IReadOnlyCollection<ITargetableActor> possibleTargets);
    }
}