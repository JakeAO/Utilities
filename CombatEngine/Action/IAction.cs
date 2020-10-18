using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Abilities;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.Action
{
    public interface IAction
    {
        uint Id { get; }
        bool Available { get; }
        IAbility Ability { get; }
        IInitiativeActor Source { get; }
        IReadOnlyCollection<ICharacterActor> Targets { get; }
    }
}