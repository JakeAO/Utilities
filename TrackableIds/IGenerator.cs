namespace SadPumpkin.Util.TrackableIds
{
    public interface IGenerator<T>
    {
        /// <summary>
        /// Constant minimum value of the range that this Generator covers.
        /// </summary>
        T Min { get; }

        /// <summary>
        /// Constant maximum value of the range that this Generator covers.
        /// </summary>
        T Max { get; }

        /// <summary>
        /// Current latest value that this Generator used.
        /// </summary>
        T Current { get; }

        /// <summary>
        /// Grab the next value in the Generator and update Current.
        /// </summary>
        /// <returns>Next trackable id in the generator.</returns>
        T GetNext();
    }
}