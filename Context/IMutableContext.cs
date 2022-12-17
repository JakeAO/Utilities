namespace SadPumpkin.Util.Context
{
    public interface IMutableContext : IContext
    {
        /// <summary>
        /// Sets a provider for a specific reference type.
        /// Providers lazy-initialize references within the Context.
        /// </summary>
        /// <param name="valueProvider">Value provider of the appropriate type.</param>
        /// <typeparam name="T">Type of reference to be added to the Context.</typeparam>
        void SetProvider<T>(IValueProvider<T> valueProvider);
        
        /// <summary>
        /// Sets a reference within the Context.
        /// </summary>
        /// <param name="value">Value to add to the Context.</param>
        /// <typeparam name="T">Type of reference to be added to the Context.</typeparam>
        void SetValue<T>(T value);

        /// <summary>
        /// Clear all references within the Context.
        /// </summary>
        /// <param name="includeProviders">Should the clear also remove all value providers.</param>
        void Clear(bool includeProviders = false);
        
        /// <summary>
        /// Clears the reference of a specific type in the Context.
        /// </summary>
        /// <param name="includeProvider">Should the clear also remove all value providers for the type.</param>
        /// <typeparam name="T">Type of reference to be added to the Context.</typeparam>
        void Clear<T>(bool includeProvider = false);
    }
}