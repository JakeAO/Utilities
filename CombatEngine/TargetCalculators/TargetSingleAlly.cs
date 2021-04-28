using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.TargetCalculators
{
    /// <summary>
    /// Targeting logic for singular Actors of the same Party.
    /// </summary>
    public class SingleAllyTargetCalculator : ITargetCalc
    {
        public static readonly SingleAllyTargetCalculator Instance = new SingleAllyTargetCalculator();

        /// <summary>
        /// User-readable description of the Targeting logic.
        /// </summary>
        public string Description { get; } = "One Ally";

        /// <summary>
        /// Determines if the given Actor is targetable or not.
        /// </summary>
        /// <param name="sourceActor">Actor doing the targeting.</param>
        /// <param name="targetActor">Actor being targeted.</param>
        /// <returns><c>True</c> if the Actor can be targeted, otherwise <c>False</c>.</returns>
        public bool CanTarget(IInitiativeActor sourceActor, ITargetableActor targetActor)
        {
            return sourceActor.Party == targetActor.Party &&
                   targetActor.IsAlive();
        }

        /// <summary>
        /// Calculate all possible targeting permutations.
        /// </summary>
        /// <param name="sourceActor">Actor doing the targeting.</param>
        /// <param name="possibleTargets">All possible targetable Actors.</param>
        /// <returns>Collection containing all possible targeting group combinations.</returns>
        public IReadOnlyCollection<IReadOnlyCollection<ITargetableActor>> GetTargetOptions(
            IInitiativeActor sourceActor,
            IReadOnlyCollection<ITargetableActor> possibleTargets)
        {
            List<IReadOnlyCollection<ITargetableActor>> targetOptions = new List<IReadOnlyCollection<ITargetableActor>>(possibleTargets.Count);
            foreach (ITargetableActor possibleTarget in possibleTargets)
            {
                if (CanTarget(sourceActor, possibleTarget))
                {
                    targetOptions.Add(new[] {possibleTarget});
                }
            }

            return targetOptions;
        }
    }
}