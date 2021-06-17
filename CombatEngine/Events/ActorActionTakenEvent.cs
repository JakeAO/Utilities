using SadPumpkin.Util.CombatEngine.Action;

namespace SadPumpkin.Util.CombatEngine.Events
{
    public readonly struct ActorActionTakenEvent : ICombatEventData
    {
        public readonly uint ActorId;
        public readonly IAction Action;

        public ActorActionTakenEvent(uint actorId, IAction action)
        {
            ActorId = actorId;
            Action = action;
        }
    }
}