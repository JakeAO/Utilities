using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.EquipMap;
using SadPumpkin.Util.CombatEngine.Item;

namespace SadPumpkin.Util.CombatEngine.StateChangeEvents
{
    public class CharacterEquipmentChangedEvent : CharacterChangeEventBase
    {
        public EquipmentSlot Slot { get; }
        public IItem OldItem { get; }
        public IItem NewItem { get; }

        public CharacterEquipmentChangedEvent(
            uint oldStateId, uint newStateId,
            ICharacterActor actor,
            EquipmentSlot slot, IItem oldItem, IItem newItem)
            : base(oldStateId, newStateId, actor)
        {
            Slot = slot;
            OldItem = oldItem;
            NewItem = newItem;

            Description = $"Actor {actor.Name}'s {slot} changed from {oldItem?.Name ?? "[Empty]"} to {newItem?.Name ?? "[Empty]"}";
        }
    }
}