namespace SadPumpkin.Util.CombatEngine.StateChangeEvents
{
    public class ActorAlivenessChangedEvent : ActorChangeEventBase
    {
        public bool OldAlive { get; }
        public bool NewAlive { get; }

        public ActorAlivenessChangedEvent(
            uint oldStateId, uint newStateId,
            uint actorId,
            bool oldAlive, bool newAlive)
            : base(oldStateId, newStateId, actorId)
        {
            OldAlive = oldAlive;
            NewAlive = newAlive;

            Description = $"Actor {actorId} has {(newAlive ? "revived" : "died")}.";
        }
    }
}