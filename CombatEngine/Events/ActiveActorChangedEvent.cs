namespace SadPumpkin.Util.CombatEngine.Events
{
    public class ActiveActorChangedEvent : ICombatEventData
    {
        public readonly uint NewActorId;

        public ActiveActorChangedEvent(uint newActorId)
        {
            NewActorId = newActorId;
        }
    }
}