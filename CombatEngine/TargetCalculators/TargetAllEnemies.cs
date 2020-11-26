using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.TargetCalculators
{
    public class AllEnemyTargetCalculator : ITargetCalc
    {
        public static readonly AllEnemyTargetCalculator Instance = new AllEnemyTargetCalculator();
        
        private AllEnemyTargetCalculator()
        {
            
        }

        public string Description { get; } = "All Enemies";

        public bool CanTarget(IInitiativeActor sourceCharacter, ITargetableActor targetCharacter)
        {
            return sourceCharacter.Party != targetCharacter.Party &&
                   targetCharacter.IsAlive();
        }

        public IReadOnlyCollection<IReadOnlyCollection<ITargetableActor>> GetTargetOptions(IInitiativeActor sourceCharacter, IReadOnlyCollection<ITargetableActor> possibleTargets)
        {
            List<ITargetableActor> allTargets = new List<ITargetableActor>(possibleTargets.Count);
            foreach (ITargetableActor possibleTarget in possibleTargets)
            {
                if (CanTarget(sourceCharacter, possibleTarget))
                {
                    allTargets.Add(possibleTarget);
                }
            }
            return new[] {allTargets};
        }
    }
}