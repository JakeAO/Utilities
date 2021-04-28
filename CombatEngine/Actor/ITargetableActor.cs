namespace SadPumpkin.Util.CombatEngine.Actor
{
    /// <summary>
    /// Interface which defines an Actor that can be targeted by an Action.
    /// </summary>
    public interface ITargetableActor : IInitiativeActor
    {
        /// <summary>
        /// Can this Actor currently be the target of Actions.
        /// </summary>
        /// <returns><c>True</c> if this Actor can be targeted, otherwise <c>False</c>.</returns>
        bool CanTarget();
    }
}