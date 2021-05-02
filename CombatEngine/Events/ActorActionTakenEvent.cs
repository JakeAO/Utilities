using SadPumpkin.Util.CombatEngine.Action;

namespace SadPumpkin.Util.CombatEngine.Events
{
    public class ActorActionTakenEvent : ICombatEventData
    {
        public readonly IAction Action;

        public ActorActionTakenEvent(IAction action)
        {
            Action = action;
        }
    }
}