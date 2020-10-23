using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.StateChangeEvents
{
    public class ActorAlivenessChangedEvent : ActorChangeEventBase
    {
        public bool OldAlive { get; }
        public bool NewAlive { get; }

        public ActorAlivenessChangedEvent(
            uint oldStateId, uint newStateId,
            IInitiativeActor actor,
            bool oldAlive, bool newAlive)
            : base(oldStateId, newStateId, actor)
        {
            OldAlive = oldAlive;
            NewAlive = newAlive;

            Description = $"Actor {actor.Name} has {(newAlive ? "revived" : "died")}.";
        }
    }
}