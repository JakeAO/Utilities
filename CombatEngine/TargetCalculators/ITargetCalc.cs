using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.TargetCalculators
{
    /// <summary>
    /// Interface defining a Targeting logic of an Action.
    /// </summary>
    public interface ITargetCalc
    {
        /// <summary>
        /// User-readable description of the Targeting logic.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Determines if the given Actor is targetable or not.
        /// </summary>
        /// <param name="sourceActor">Actor doing the targeting.</param>
        /// <param name="targetActor">Actor being targeted.</param>
        /// <returns><c>True</c> if the Actor can be targeted, otherwise <c>False</c>.</returns>
        bool CanTarget(IInitiativeActor sourceActor, ITargetableActor targetActor);

        /// <summary>
        /// Calculate all possible targeting permutations.
        /// </summary>
        /// <param name="sourceActor">Actor doing the targeting.</param>
        /// <param name="possibleTargets">All possible targetable Actors.</param>
        /// <returns>Collection containing all possible targeting group combinations.</returns>
        IReadOnlyCollection<IReadOnlyCollection<ITargetableActor>> GetTargetOptions(IInitiativeActor sourceActor, IReadOnlyCollection<ITargetableActor> possibleTargets);
    }
}