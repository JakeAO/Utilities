using SadPumpkin.Util.CombatEngine.StatMap;

namespace SadPumpkin.Util.CombatEngine.StateChangeEvents
{
    public class CharacterStatChangedEvent : ActorChangeEventBase
    {
        public StatType Stat { get; }
        public uint OldValue { get; }
        public uint NewValue { get; }

        public CharacterStatChangedEvent(
            uint oldStateId, uint newStateId,
            uint actorId,
            StatType stat, uint oldValue, uint newValue)
            : base(oldStateId, newStateId, actorId)
        {
            Stat = stat;
            OldValue = oldValue;
            NewValue = newValue;

            Description = $"Actor {actorId} {stat} changed from {oldValue} to {newValue}";
        }
    }
}