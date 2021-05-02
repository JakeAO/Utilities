namespace SadPumpkin.Util.CombatEngine.Events
{
    public class CombatCompletedEvent : ICombatEventData
    {
        public readonly uint WinningPartyId;

        public CombatCompletedEvent(uint winningPartyId)
        {
            WinningPartyId = winningPartyId;
        }
    }
}