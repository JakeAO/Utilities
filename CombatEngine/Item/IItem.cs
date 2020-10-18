using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Abilities;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.Item
{
    public interface IItem : IIdTracked
    {
        string Name { get; }
        string Desc { get; }
        ItemType ItemType { get; }
        IReadOnlyCollection<IAbility> AddedAbilities { get; }

        IReadOnlyCollection<IAction> GetAllActions(ICharacterActor sourceCharacter, IReadOnlyCollection<ITargetableActor> possibleTargets, bool isEquipped);

        // TODO Stat Bonus
    }
}