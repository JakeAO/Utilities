namespace SadPumpkin.Util.LootTable
{
    /// <summary>
    /// Interface for an ILootEntry which contains a generic value.
    /// </summary>
    /// <typeparam name="T">Type of the value contained by the entry.</typeparam>
    public interface IValueLootEntry<T> : ILootEntry
    {
        /// <summary>
        /// Value which this loot entry holds.
        /// </summary>
        T Value { get; }
    }
}