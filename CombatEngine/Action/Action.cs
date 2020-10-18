using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Abilities;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.Action
{
    public class Action : IAction
    {
        public uint Id { get; }
        public bool Available { get; }
        public IAbility Ability { get; }
        public IInitiativeActor Source { get; }
        public IReadOnlyCollection<ICharacterActor> Targets { get; }

        public Action(
            uint id,
            bool available,
            IAbility ability,
            IInitiativeActor source,
            IReadOnlyCollection<ICharacterActor> targets)
        {
            Id = id;
            Available = available;
            Ability = ability;
            Source = source;
            Targets = targets;
        }
    }
}