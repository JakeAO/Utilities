using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.Events;

namespace SadPumpkin.Util.CombatEngine.ActorChangeCalculator
{
    public interface IActorChangeCalculator
    {
        IEnumerable<ICombatEventData> GetChangeEvents(IInitiativeActor before, IInitiativeActor after);
    }
}