using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.TargetCalculators
{
    /// <summary>
    /// Targeting logic for Actions targeting the source Actor.
    /// </summary>
    public class SelfTargetCalculator : ITargetCalc
    {
        public static readonly SelfTargetCalculator Instance = new SelfTargetCalculator();

        /// <summary>
        /// User-readable description of the Targeting logic.
        /// </summary>
        public string Description { get; } = "Self";

        /// <summary>
        /// Determines if the given Actor is targetable or not.
        /// </summary>
        /// <param name="sourceActor">Actor doing the targeting.</param>
        /// <param name="targetActor">Actor being targeted.</param>
        /// <returns><c>True</c> if the Actor can be targeted, otherwise <c>False</c>.</returns>
        public bool CanTarget(IInitiativeActor sourceActor, ITargetableActor targetActor)
        {
            return sourceActor.Id == targetActor.Id;
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
            if (sourceActor is ITargetableActor targetableSource)
            {
                targetOptions.Add(new[] {targetableSource});
            }

            return targetOptions;
        }
    }
}