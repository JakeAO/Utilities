namespace SadPumpkin.Util.CombatEngine.StateChangeEvents
{
    public abstract class ActorChangeEventBase : StateChangeEventBase
    {
        public uint ActorId { get; }

        protected ActorChangeEventBase(uint oldStateId, uint newStateId, uint actorId)
            : base(oldStateId, newStateId)
        {
            ActorId = actorId;

            Description = $"No description for event type {GetType().Name} for actor {actorId} from state {oldStateId} to {newStateId}";
        }
    }
}