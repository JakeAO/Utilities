using System.Collections.Generic;
using System.Linq;

namespace SadPumpkin.Util.CombatEngine.InitiativeSorter
{
    public class ThresholdInitiativeSorter : IInitiativeSorter
    {
        private readonly float _initiativeThreshold;
        private readonly List<IInitiativePair> _shadowCopy = new List<IInitiativePair>(10);

        public ThresholdInitiativeSorter(float initiativeThreshold = 100f)
        {
            _initiativeThreshold = initiativeThreshold;
        }

        public IInitiativePair GetNext(IEnumerable<IInitiativePair> initiativePairs)
        {
            _shadowCopy.AddRange(initiativePairs);

            IInitiativePair next = null;
            do
            {
                _shadowCopy.Sort(CompareInitiativePair);
                if (_shadowCopy[0].Initiative >= _initiativeThreshold)
                {
                    next = _shadowCopy[0];
                }
                else if (!IncrementInitiativePairs(_shadowCopy))
                {
                    // Failed to increment, we're going to be looping forever.
                    break;
                }
            } while (next == null);

            _shadowCopy.Clear();
            return next;
        }

        public IEnumerable<IInitiativePair> PredictNext(IEnumerable<IInitiativePair> initiativePairs, uint count)
        {
            // make copies, we're predicting and don't want to modify the originals
            _shadowCopy.AddRange(initiativePairs.Select(p => p.Copy()));

            for (int i = 0; i < count; i++)
            {
                // get next using normal means
                // make copy, we're only keeping one shadow copy internally so if an actor shows up twice we don't want its initiative to be wrong
                yield return GetNext(_shadowCopy).Copy();
            }

            _shadowCopy.Clear();
        }

        private static bool IncrementInitiativePairs(IEnumerable<IInitiativePair> initiativePairs)
        {
            bool success = false;
            foreach (IInitiativePair initiativePair in initiativePairs)
            {
                if (initiativePair is IWritableInitiativePair writableInitiativePair)
                {
                    writableInitiativePair.IncrementInitiative(initiativePair.Entity.GetInitiative());
                    success = true;
                }
            }

            return success;
        }

        private static int CompareInitiativePair(IInitiativePair lhs, IInitiativePair rhs)
            => rhs.Initiative.CompareTo(lhs.Initiative);
    }
}