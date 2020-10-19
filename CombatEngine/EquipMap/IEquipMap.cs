using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.Item;
using SadPumpkin.Util.CombatEngine.Item.Armors;
using SadPumpkin.Util.CombatEngine.Item.Weapons;

namespace SadPumpkin.Util.CombatEngine.EquipMap
{
    public interface IEquipMap : ICopyable<IEquipMap>
    {
        IWeapon Weapon { get; }
        IArmor Armor { get; }
        IItem ItemA { get; }
        IItem ItemB { get; }

        IItem this[EquipmentSlot slot] { get; }
        
        IReadOnlyCollection<IAction> GetAllActions(ICharacterActor activeChar, IReadOnlyCollection<ITargetableActor> possibleTargets);
    }
}