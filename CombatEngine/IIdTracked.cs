namespace SadPumpkin.Util.CombatEngine
{
    /// <summary>
    /// Interface which defines an object that can be tracked by a unique Id.
    /// </summary>
    public interface IIdTracked
    {
        /// <summary>
        /// Unique Id used to track this object.
        /// </summary>
        uint Id { get; }
    }
}