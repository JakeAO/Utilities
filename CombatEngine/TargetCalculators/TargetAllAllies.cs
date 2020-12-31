using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.TargetCalculators
{
    /// <summary>
    /// Targeting logic for all Actors of the same Party.
    /// </summary>
    public class AllAllyTargetCalculator : ITargetCalc
    {
        public static readonly AllAllyTargetCalculator Instance = new AllAllyTargetCalculator();

        /// <summary>
        /// User-readable description of the Targeting logic.
        /// </summary>
        public string Description { get; } = "All Allies";

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
            List<ITargetableActor> allTargets = new List<ITargetableActor>(possibleTargets.Count);
            foreach (ITargetableActor possibleTarget in possibleTargets)
            {
                if (CanTarget(sourceActor, possibleTarget))
                {
                    allTargets.Add(possibleTarget);
                }
            }

            return new[] {allTargets};
        }
    }
}