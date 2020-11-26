using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.CostCalculators;
using SadPumpkin.Util.CombatEngine.EffectCalculators;

namespace SadPumpkin.Util.CombatEngine.Action
{
    public interface IAction
    {
        uint Id { get; }
        bool Available { get; }
        IInitiativeActor Source { get; }
        IReadOnlyCollection<ITargetableActor> Targets { get; }
        uint Speed { get; }
        ICostCalc Cost { get; }
        IEffectCalc Effect { get; }
        IIdTracked ActionSource { get; }
    }
}