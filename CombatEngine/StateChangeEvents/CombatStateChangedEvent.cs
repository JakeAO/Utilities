namespace SadPumpkin.Util.CombatEngine.StateChangeEvents
{
    public class CombatStateChangedEvent : StateChangeEventBase
    {
        public CombatState OldCombatState { get; }
        public CombatState NewCombatState { get; }

        public CombatStateChangedEvent(
            uint oldStateId, uint newStateId,
            CombatState oldCombatState, CombatState newCombatState)
            : base(oldStateId, newStateId)
        {
            OldCombatState = oldCombatState;
            NewCombatState = newCombatState;

            Description = $"CombatState changed from {OldCombatState} to {NewCombatState}";
        }
    }
}