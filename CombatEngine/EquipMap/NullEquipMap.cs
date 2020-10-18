using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.Item;
using SadPumpkin.Util.CombatEngine.Item.Armors;
using SadPumpkin.Util.CombatEngine.Item.Weapons;

namespace SadPumpkin.Util.CombatEngine.EquipMap
{
    public class NullEquipMap : IEquipMap
    {
        public static NullEquipMap Instance = new NullEquipMap();

        private NullEquipMap()
        {
        }
        
        public IWeapon Weapon => null;
        public IArmor Armor => null;
        public IItem ItemA => null;
        public IItem ItemB => null;

        public IItem this[EquipmentSlot slot] => null;

        public IReadOnlyCollection<IAction> GetAllActions(ICharacterActor activeChar, IReadOnlyCollection<ITargetableActor> possibleTargets) => new IAction[0];
    }
}