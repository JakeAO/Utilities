using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.RequirementCalculators
{
    /// <summary>
    /// Interface defining a calculator which determines if an Actor meets
    /// the requirements to perform an Action.
    /// </summary>
    public interface IRequirementCalc
    {
        /// <summary>
        /// User-readable description of the Requirement.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Does the provided Actor meet this Requirement.
        /// </summary>
        /// <param name="actor">Actor which will take the Action.</param>
        /// <returns><c>True</c> if the Requirement is met, otherwise <c>False</c>.</returns>
        bool MeetsRequirement(IInitiativeActor actor);
    }
}