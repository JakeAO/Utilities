using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Abilities;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.Item.Weapons
{
    public class Weapon : IWeapon
    {
        public uint Id { get; }
        public string Name { get; }
        public string Desc { get; }
        public ItemType ItemType { get; }
        public WeaponType WeaponType { get; }
        public IReadOnlyCollection<IAbility> AddedAbilities { get; }

        private readonly IAbility _attackAbility;

        public Weapon()
            : this(0,
                string.Empty, string.Empty,
                WeaponType.Invalid,
                null,
                null)
        {
        }

        public Weapon(
            uint id,
            string name, string desc,
            WeaponType weaponType,
            IAbility attackAbility,
            IReadOnlyCollection<IAbility> addedAbilities)
        {
            Id = id;
            Name = name;
            Desc = desc;
            ItemType = ItemType.Weapon;
            WeaponType = weaponType;
            AddedAbilities = addedAbilities != null
                ? new List<IAbility>(addedAbilities)
                : new List<IAbility>();

            _attackAbility = attackAbility;
        }

        public IReadOnlyCollection<IAction> GetAllActions(ICharacterActor sourceCharacter, IReadOnlyCollection<ITargetableActor> possibleTargets, bool isEquipped)
        {
            List<IAction> actions = new List<IAction>(10);

            if (_attackAbility != null && isEquipped)
            {
                actions.AddRange(ActionUtil.GetActionsForAbility(_attackAbility, sourceCharacter, possibleTargets));
            }

            if (AddedAbilities != null)
            {
                foreach (IAbility ability in AddedAbilities)
                {
                    actions.AddRange(ActionUtil.GetActionsForAbility(ability, sourceCharacter, possibleTargets));
                }
            }

            return actions;
        }
    }
}