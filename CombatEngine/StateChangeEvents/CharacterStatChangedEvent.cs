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

            int change = (int) newValue - (int) oldValue;
            if (change > 0)
            {
                Description = $"{actor.Name}'s {stat} increased by {change}";
            }
            else if (change < 0)
            {
                Description = $"{actor.Name}'s {stat} decreased by {-change}";
            }
        }
    }
}