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

        public string Description { get; } = "Self";

        public bool CanTarget(IInitiativeActor sourceCharacter, ITargetableActor targetCharacter)
        {
            return sourceCharacter.Id == targetCharacter.Id;
        }

        public IReadOnlyCollection<IReadOnlyCollection<ITargetableActor>> GetTargetOptions(
            IInitiativeActor sourceCharacter, IReadOnlyCollection<ITargetableActor> possibleTargets)
        {
            List<IReadOnlyCollection<ITargetableActor>> targetOptions = new List<IReadOnlyCollection<ITargetableActor>>(possibleTargets.Count);
            if (sourceCharacter is ITargetableActor targetableSource)
            {
                targetOptions.Add(new[] {targetableSource});
            }

            return targetOptions;
        }
    }
}