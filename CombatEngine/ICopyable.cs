namespace SadPumpkin.Util.CombatEngine
{
    /// <summary>
    /// Interface which defines an object that can create a deep copy of itself.
    /// </summary>
    /// <typeparam name="T">Type of object.</typeparam>
    public interface ICopyable<T>
    {
        /// <summary>
        /// Create a deep copy of the current object.
        /// </summary>
        /// <returns>Duplicate of the current object.</returns>
        T Copy();
    }
}