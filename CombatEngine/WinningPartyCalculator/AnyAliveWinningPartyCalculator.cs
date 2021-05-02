using System;
using System.Collections.Generic;
using System.Linq;
using SadPumpkin.Util.CombatEngine.Party;

namespace SadPumpkin.Util.CombatEngine.WinningPartyCalculator
{
    public class AnyAliveWinningPartyCalculator : IWinningPartyCalculator
    {
        public bool GetWinningPartyId(IEnumerable<IParty> parties, out uint winningParty)
        {
            try
            {
                winningParty = parties.Single(p => p.Actors.Any(a => a.IsAlive())).Id;
                return true;
            }
            catch (InvalidOperationException)
            {
                // LINQ throws an InvalidOperationException when:
                //      - the collection has no matches.
                //      - the collection has more than 1 match.
                // Both of these are expected, we only want to return True if there is a SINGLE result.
                winningParty = 0u;
                return false;
            }
        }
    }
}