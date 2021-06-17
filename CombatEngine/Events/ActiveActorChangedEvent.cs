namespace SadPumpkin.Util.CombatEngine.Events
{
    public readonly struct ActiveActorChangedEvent : ICombatEventData
    {
        public readonly uint NewActorId;

        public ActiveActorChangedEvent(uint newActorId)
        {
            NewActorId = newActorId;
        }
    }
}