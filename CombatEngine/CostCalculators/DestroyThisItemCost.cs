using System;
using SadPumpkin.Util.CombatEngine.Abilities;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.EquipMap;
using SadPumpkin.Util.CombatEngine.Item;

namespace SadPumpkin.Util.CombatEngine.CostCalculators
{
    public class DestroyThisItemCost : ICostCalc
    {
        public string Key { get; }

        public DestroyThisItemCost(string uniqueItemKey)
        {
            Key = uniqueItemKey;
        }

        public bool CanAfford(IInitiativeActor entity)
        {
            if (entity is IPlayerCharacterActor playerCharacter)
            {
                foreach (EquipmentSlot slot in Enum.GetValues(typeof(EquipmentSlot)))
                {
                    if (playerCharacter.Equipment[slot] is IItem item)
                    {
                        foreach (IAbility addedAbility in item.AddedAbilities)
                        {
                            if (addedAbility.Cost is DestroyThisItemCost destroyItemCost &&
                                string.Equals(Key, destroyItemCost.Key))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public void Pay(IInitiativeActor entity)
        {
            if (entity is IPlayerCharacterActor playerCharacter &&
                playerCharacter.Equipment is EquipMap.EquipMap rawEquipMap)
            {
                foreach (EquipmentSlot slot in Enum.GetValues(typeof(EquipmentSlot)))
                {
                    if (rawEquipMap[slot] is IItem item)
                    {
                        foreach (IAbility addedAbility in item.AddedAbilities)
                        {
                            if (addedAbility.Cost is DestroyThisItemCost destroyItemCost &&
                                string.Equals(Key, destroyItemCost.Key))
                            {
                                switch (slot)
                                {
                                    case EquipmentSlot.Weapon:
                                        rawEquipMap.Weapon = null;
                                        break;
                                    case EquipmentSlot.Armor:
                                        rawEquipMap.Armor = null;
                                        break;
                                    case EquipmentSlot.ItemA:
                                        rawEquipMap.ItemA = null;
                                        break;
                                    case EquipmentSlot.ItemB:
                                        rawEquipMap.ItemB = null;
                                        break;
                                }

                                return;
                            }
                        }
                    }
                }
            }
        }

        public string Description()
        {
            return "Consumes Item";
        }
    }
}