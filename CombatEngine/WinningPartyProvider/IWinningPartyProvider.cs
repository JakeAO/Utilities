using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Party;

namespace SadPumpkin.Util.CombatEngine.WinningPartyProvider
{
    public interface IWinningPartyProvider
    {
        bool TryGetWinner(IReadOnlyCollection<IParty> parties, out uint winningPartyId);
    }
}