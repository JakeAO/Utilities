namespace SadPumpkin.Util.CombatEngine.Events
{
    public readonly struct CombatStateChangedEvent : ICombatEventData
    {
        public readonly CombatState NewState;

        public CombatStateChangedEvent(CombatState newState)
        {
            NewState = newState;
        }
    }
}