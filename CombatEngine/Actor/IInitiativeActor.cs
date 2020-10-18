using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Action;

namespace SadPumpkin.Util.CombatEngine.Actor
{
    public interface IInitiativeActor
    {
        uint Id { get; }
        uint Party { get; }
        string Name { get; }

        bool IsAlive();
        float GetInitiative();
        
        IReadOnlyCollection<IAction> GetAllActions(IReadOnlyCollection<ITargetableActor> possibleTargets);
    }
}