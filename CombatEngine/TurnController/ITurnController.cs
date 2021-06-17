using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.CharacterControllers;

namespace SadPumpkin.Util.CombatEngine.TurnController
{
    public interface ITurnController
    {
        IEnumerable<IAction> TakeTurn(
            IInitiativeActor activeActor, 
            ICharacterController actorController, 
            IReadOnlyCollection<ITargetableActor> allTargets,
            IStandardActionGenerator standardActionGenerator);
    }
}