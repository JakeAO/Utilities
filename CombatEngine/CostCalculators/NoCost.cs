using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.CostCalculators
{
    /// <summary>
    /// Implementation of a Cost calculator with no Cost.
    /// </summary>
    public class NoCost : ICostCalc
    {
        public static readonly NoCost Instance = new NoCost();

        /// <summary>
        /// User-readable description of the Cost.
        /// </summary>
        public string Description => "None";

        /// <summary>
        /// Can the provided Actor currently afford the Cost.
        /// </summary>
        /// <param name="entity">Actor to determine Cost for.</param>
        /// <param name="actionSource">Source object which provides the Action.</param>
        /// <returns><c>True</c> if the Cost can be paid, otherwise <c>False</c>.</returns>
        public bool CanAfford(IInitiativeActor entity, IIdTracked actionSource)
        {
            return true;
        }

        /// <summary>
        /// Pay this Cost with the provided Actor.
        /// </summary>
        /// <param name="entity">Actor to pay the Cost with.</param>
        /// <param name="actionSource">Source object which provided the Action.</param>
        public void Pay(IInitiativeActor entity, IIdTracked actionSource)
        {
            // Intentionally left blank
        }
    }
}