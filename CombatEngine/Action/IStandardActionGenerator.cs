using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.Action
{
    public interface IStandardActionGenerator
    {
        IEnumerable<IAction> GetActions(IInitiativeActor actor);
    }
}