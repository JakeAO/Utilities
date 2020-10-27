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

            if (NewItem != null)
            {
                Description = $"{NewItem?.Name} equipped to {actor.Name}'s {slot} slot.";
            }
            else
            {
                Description = $"{OldItem?.Name} unequipped from {actor.Name}'s {slot} slot.";
            }
        }
    }
}