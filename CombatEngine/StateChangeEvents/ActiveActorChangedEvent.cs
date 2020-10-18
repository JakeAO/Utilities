namespace SadPumpkin.Util.CombatEngine.StateChangeEvents
{
    public class ActiveActorChangedEvent : StateChangeEventBase
    {
        public uint? OldActorId { get; }
        public uint? NewActorId { get; }

        public ActiveActorChangedEvent(
            uint oldStateId, uint newStateId,
            uint? oldActorId, uint? newActorId)
            : base(oldStateId, newStateId)
        {
            OldActorId = oldActorId;
            NewActorId = newActorId;

            Description = $"ActiveActor changed from {oldActorId} to {newActorId}";
        }
    }
}