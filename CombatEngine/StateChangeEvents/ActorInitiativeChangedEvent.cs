namespace SadPumpkin.Util.CombatEngine.StateChangeEvents
{
    public class ActorInitiativeChangedEvent : ActorChangeEventBase
    {
        public float OldInitiative { get; }
        public float NewInitiative { get; }

        public ActorInitiativeChangedEvent(
            uint oldStateId, uint newStateId,
            uint actorId,
            float oldInitiative, float newInitiative)
            : base(oldStateId, newStateId, actorId)
        {
            OldInitiative = oldInitiative;
            NewInitiative = newInitiative;

            Description = $"Actor {actorId} initiative changed from {oldInitiative} to {newInitiative}";
        }
    }
}