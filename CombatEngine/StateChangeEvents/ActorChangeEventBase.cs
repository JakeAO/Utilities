using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.StateChangeEvents
{
    public abstract class ActorChangeEventBase : StateChangeEventBase
    {
        public IInitiativeActor Actor { get; }

        protected ActorChangeEventBase(uint oldStateId, uint newStateId, IInitiativeActor actor)
            : base(oldStateId, newStateId)
        {
            Actor = actor;

            Description = $"No description for event type {GetType().Name} for actor {actor.Name} from state {oldStateId} to {newStateId}";
        }
    }
}