using System;
using System.Collections.Generic;
using System.Linq;
using SadPumpkin.Util.CombatEngine.Party;

namespace SadPumpkin.Util.CombatEngine.WinningPartyProvider
{
    public class OneAliveWinningPartyProvider : IWinningPartyProvider
    {
        public bool TryGetWinner(IReadOnlyCollection<IParty> parties, out uint winningPartyId)
        {
            try
            {
                IParty winningParty = parties.Single(p => p.Actors.Any(a => a.IsAlive()));
                winningPartyId = winningParty.Id;
                return true;
            }
            catch (InvalidOperationException) // .Single() throws an exception if > 1 matches the predicate
            {
                winningPartyId = 0u;
                return false;
            }
        }
    }
}