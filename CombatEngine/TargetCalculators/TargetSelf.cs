using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.TargetCalculators
{
    public class SelfTargetCalculator : ITargetCalc
    {
        public static readonly SelfTargetCalculator Instance = new SelfTargetCalculator();

        private SelfTargetCalculator()
        {

        }

        public bool CanTarget(ICharacterActor sourceCharacter, ICharacterActor targetCharacter)
        {
            return sourceCharacter.Id == targetCharacter.Id;
        }

        public IReadOnlyCollection<IReadOnlyCollection<ICharacterActor>> GetTargetOptions(ICharacterActor sourceCharacter, IReadOnlyCollection<ITargetableActor> possibleTargets)
        {
            return new[] {new[] {sourceCharacter}};
        }
    }
}