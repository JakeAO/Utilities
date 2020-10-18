using SadPumpkin.Util.CombatEngine.EquipMap;

namespace SadPumpkin.Util.CombatEngine.StateChangeEvents
{
    public class CharacterEquipmentChangedEvent : ActorChangeEventBase
    {
        public EquipmentSlot Slot { get; }
        public uint? OldItemId { get; }
        public uint? NewItemId { get; }

        public CharacterEquipmentChangedEvent(
            uint oldStateId, uint newStateId,
            uint actorId,
            EquipmentSlot slot, uint? oldItemId, uint? newItemId)
            : base(oldStateId, newStateId, actorId)
        {
            Slot = slot;
            OldItemId = oldItemId;
            NewItemId = newItemId;

            Description = $"Actor {actorId} {slot} changed from {oldItemId} to {newItemId}";
        }
    }
}