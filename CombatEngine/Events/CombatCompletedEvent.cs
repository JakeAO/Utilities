namespace SadPumpkin.Util.CombatEngine.Events
{
    public readonly struct CombatCompletedEvent : ICombatEventData
    {
        public readonly uint WinningPartyId;

        public CombatCompletedEvent(uint winningPartyId)
        {
            WinningPartyId = winningPartyId;
        }
    }
}