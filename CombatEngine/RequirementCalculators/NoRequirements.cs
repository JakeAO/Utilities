using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.RequirementCalculators
{
    /// <summary>
    /// Implementation of a Requirement calculator with no Requirements.
    /// </summary>
    public class NoRequirements : IRequirementCalc
    {
        public static readonly NoRequirements Instance = new NoRequirements();

        /// <summary>
        /// User-readable description of the Requirement.
        /// </summary>
        public string Description => "None";
        
        /// <summary>
        /// Does the provided Actor meet this Requirement.
        /// </summary>
        /// <param name="actor">Actor which will take the Action.</param>
        /// <returns><c>True</c> if the Requirement is met, otherwise <c>False</c>.</returns>
        public bool MeetsRequirement(IInitiativeActor actor)
        {
            return true;
        }
    }
}