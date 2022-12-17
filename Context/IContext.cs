namespace SadPumpkin.Util.Context
{
    public interface IContext
    {
        /// <summary>
        /// Pull the requested type from the Context.
        /// </summary>
        /// <param name="exact">Should the match include inherited types or exactly match the requested type.</param>
        /// <typeparam name="T">Type of reference to pull from the Context.</typeparam>
        /// <returns>A reference to the object or that type's default value.</returns>
        T Get<T>(bool exact = false);
        
        /// <summary>
        /// Pull the requested type from the Context if it exists.
        /// </summary>
        /// <param name="value">A reference of the requested type.</param>
        /// <param name="exact">Should the match include inherited types or exactly match the requested type.</param>
        /// <typeparam name="T">Type of reference to pull from the Context.</typeparam>
        /// <returns><c>True</c> if the Context contains a reference, otherwise <c>False</c>.</returns>
        bool TryGet<T>(out T value, bool exact = false);
    }
}