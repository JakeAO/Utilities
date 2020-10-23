using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.StatMap;

namespace SadPumpkin.Util.CombatEngine.StateChangeEvents
{
    public class CharacterStatChangedEvent : CharacterChangeEventBase
    {
        public StatType Stat { get; }
        public uint OldValue { get; }
        public uint NewValue { get; }

        public CharacterStatChangedEvent(
            uint oldStateId, uint newStateId,
            ICharacterActor actor,
            StatType stat, uint oldValue, uint newValue)
            : base(oldStateId, newStateId, actor)
        {
            Stat = stat;
            OldValue = oldValue;
            NewValue = newValue;

            Description = $"Actor {actor.Name}'s {stat} changed from {oldValue} to {newValue}";
        }
    }
}