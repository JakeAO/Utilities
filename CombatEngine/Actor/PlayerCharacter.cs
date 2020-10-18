﻿using System.Collections.Generic;
using System.Linq;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.CharacterClasses;
using SadPumpkin.Util.CombatEngine.EquipMap;
using SadPumpkin.Util.CombatEngine.Item.Weapons;
using SadPumpkin.Util.CombatEngine.StatMap;

namespace SadPumpkin.Util.CombatEngine.Actor
{
    public class PlayerCharacter : Character, IPlayerCharacterActor
    {
        public IEquipMap Equipment { get; set; }

        public PlayerCharacter()
            : this(0, 0, string.Empty, NullClass.Instance, new StatMap.StatMap(),
                new EquipMap.EquipMap())
        {

        }

        public PlayerCharacter(
            uint id,
            uint party,
            string name,
            IPlayerClass characterClass,
            IStatMap stats,
            IEquipMap equipment)
            : base(
                id,
                party,
                name,
                characterClass,
                stats)
        {
            Equipment = equipment;
        }

        public override float GetReducedDamage(float damageAmount, DamageType damageType)
        {
            float modifiedDamage = base.GetReducedDamage(damageAmount, damageType);

            return Equipment.Armor?.GetReducedDamage(modifiedDamage, damageType) ?? modifiedDamage;
        }

        public override IReadOnlyCollection<IAction> GetAllActions(IReadOnlyCollection<ITargetableActor> possibleTargets)
        {
            var allActions = base.GetAllActions(possibleTargets).ToList();
            allActions.InsertRange(0, Equipment.GetAllActions(this, possibleTargets));
            return allActions;
        }
    }
}