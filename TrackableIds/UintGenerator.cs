using System;

namespace SadPumpkin.Util.TrackableIds
{
    public class UintGenerator : IGenerator<uint>
    {
        /// <summary>
        /// Constant minimum value of the range that this Generator covers.
        /// </summary>
        public uint Min { get; }

        /// <summary>
        /// Constant maximum value of the range that this Generator covers.
        /// </summary>
        public uint Max { get; }

        /// <summary>
        /// Current latest value that this Generator used.
        /// </summary>
        public uint Current { get; private set; }

        /// <summary>
        /// Should the Current value wrap back around to Min if Max is exceeded,
        /// or should it throw an exception?
        /// </summary>
        private readonly bool _wrapIds;
        
        /// <summary>
        /// Construct a new Generator with set minimum and count.
        /// </summary>
        /// <param name="min">Minimum/starting value of the generator.</param>
        /// <param name="count">Maximum number of ids to assign.</param>
        public UintGenerator(uint min, uint count = 1000, bool wrapIds = true)
        {
            Min = min;
            Max = min + count - 1;
            Current = min;

            _wrapIds = wrapIds;
        }

        /// <summary>
        /// Grab the next value in the Generator and update Current.
        /// </summary>
        /// <returns>Next tracked id in the generator.</returns>
        public uint GetNext()
        {
            if (Current > Max)
            {
                if (_wrapIds)
                {
                    Current = Min;
                }
                else
                {
                    throw new IndexOutOfRangeException(
                        $"Current value of IGenerator ({Current}) exceeds the maximum value of {Max}");
                }
            }

            uint next = Current;
            Current++;
            return next;
        }
    }
}