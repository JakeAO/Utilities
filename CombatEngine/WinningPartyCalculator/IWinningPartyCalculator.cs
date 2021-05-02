using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Party;

namespace SadPumpkin.Util.CombatEngine.WinningPartyCalculator
{
    public interface IWinningPartyCalculator
    {
        bool GetWinningPartyId(IEnumerable<IParty> parties, out uint winningParty);
    }
}