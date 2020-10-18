namespace SadPumpkin.Util.CombatEngine.StateChangeEvents
{
    public abstract class StateChangeEventBase : IStateChangeEvent
    {
        public uint OldStateId { get; }
        public uint NewStateId { get; }

        public string Description { get; protected set; }

        protected StateChangeEventBase(uint oldStateId, uint newStateId)
        {
            OldStateId = oldStateId;
            NewStateId = newStateId;

            Description = $"No description for event type {GetType().Name} from state {oldStateId} to {newStateId}";
        }
    }
}