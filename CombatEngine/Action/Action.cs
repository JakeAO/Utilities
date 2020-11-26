using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Abilities;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.CostCalculators;
using SadPumpkin.Util.CombatEngine.EffectCalculators;

namespace SadPumpkin.Util.CombatEngine.Action
{
    public class Action : IAction
    {
        public uint Id { get; }
        public bool Available { get; }
        public IInitiativeActor Source { get; }
        public IReadOnlyCollection<ITargetableActor> Targets { get; }
        
        public uint Speed { get; }
        public ICostCalc Cost { get; }
        public IEffectCalc Effect { get; }
        public IIdTracked ActionSource { get; }

        public Action(
            uint id,
            bool available,
            IAbility ability,
            IInitiativeActor source,
            IReadOnlyCollection<ITargetableActor> targets,
            IIdTracked actionSource)
        {
            Id = id;
            Available = available;
            Source = source;
            Targets = targets;

            Speed = ability.Speed;
            Cost = ability.Cost;
            Effect = ability.Effect;
            ActionSource = actionSource;
        }
    }
}