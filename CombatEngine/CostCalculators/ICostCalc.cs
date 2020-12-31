using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.CostCalculators
{
    /// <summary>
    /// Interface defining the Cost associated with an Action.
    /// </summary>
    public interface ICostCalc
    {
        /// <summary>
        /// User-readable description of the Cost.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Can the provided Actor currently afford the Cost.
        /// </summary>
        /// <param name="entity">Actor to determine Cost for.</param>
        /// <param name="actionSource">Source object which provides the Action.</param>
        /// <returns><c>True</c> if the Cost can be paid, otherwise <c>False</c>.</returns>
        bool CanAfford(IInitiativeActor entity, IIdTracked actionSource);

        /// <summary>
        /// Pay this Cost with the provided Actor.
        /// </summary>
        /// <param name="entity">Actor to pay the Cost with.</param>
        /// <param name="actionSource">Source object which provided the Action.</param>
        void Pay(IInitiativeActor entity, IIdTracked actionSource);
    }
}