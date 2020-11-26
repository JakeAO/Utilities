using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.Action
{
    public class NullStandardActionGenerator : IStandardActionGenerator
    {
        public IEnumerable<IAction> GetActions(IInitiativeActor actor)
        {
            yield break;
        }
    }
}