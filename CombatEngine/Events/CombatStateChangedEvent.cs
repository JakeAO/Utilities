namespace SadPumpkin.Util.CombatEngine.Events
{
    public class CombatStateChangedEvent : ICombatEventData
    {
        public readonly CombatState NewState;

        public CombatStateChangedEvent(CombatState newState)
        {
            NewState = newState;
        }
    }
}