using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.TargetCalculators
{
    public interface ITargetCalc
    {
        string Description { get; }
        bool CanTarget(IInitiativeActor sourceActor, ITargetableActor targetActor);
        IReadOnlyCollection<IReadOnlyCollection<ITargetableActor>> GetTargetOptions(IInitiativeActor sourceActor, IReadOnlyCollection<ITargetableActor> possibleTargets);
    }
}