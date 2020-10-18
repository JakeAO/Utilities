using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.TargetCalculators
{
    public class SingleEnemyTargetCalculator : ITargetCalc
    {
        public static readonly SingleEnemyTargetCalculator Instance = new SingleEnemyTargetCalculator();

        private SingleEnemyTargetCalculator()
        {

        }

        public bool CanTarget(ICharacterActor sourceCharacter, ICharacterActor targetCharacter)
        {
            return sourceCharacter.Party != targetCharacter.Party &&
                   targetCharacter.IsAlive();
        }

        public IReadOnlyCollection<IReadOnlyCollection<ICharacterActor>> GetTargetOptions(ICharacterActor sourceCharacter, IReadOnlyCollection<ITargetableActor> possibleTargets)
        {
            List<IReadOnlyCollection<ICharacterActor>> targetOptions = new List<IReadOnlyCollection<ICharacterActor>>(possibleTargets.Count);
            foreach (ITargetableActor possibleTarget in possibleTargets)
            {
                if (!(possibleTarget is ICharacterActor targetCharacter))
                    continue;

                if (CanTarget(sourceCharacter, targetCharacter))
                {
                    targetOptions.Add(new[] {targetCharacter});
                }
            }
            return targetOptions;
        }
    }
}