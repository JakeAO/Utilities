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

        public string Description { get; } = "One Enemy";

        public bool CanTarget(IInitiativeActor sourceCharacter, ITargetableActor targetCharacter)
        {
            return sourceCharacter.Party != targetCharacter.Party &&
                   targetCharacter.IsAlive();
        }

        public IReadOnlyCollection<IReadOnlyCollection<ITargetableActor>> GetTargetOptions(IInitiativeActor sourceCharacter, IReadOnlyCollection<ITargetableActor> possibleTargets)
        {
            List<IReadOnlyCollection<ITargetableActor>> targetOptions = new List<IReadOnlyCollection<ITargetableActor>>(possibleTargets.Count);
            foreach (ITargetableActor possibleTarget in possibleTargets)
            {
                if (CanTarget(sourceCharacter, possibleTarget))
                {
                    targetOptions.Add(new[] {possibleTarget});
                }
            }
            return targetOptions;
        }
    }
}