using System;
using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.CharacterControllers
{
    public interface ICharacterController
    {
        void SelectAction(IInitiativeActor activeEntity, IReadOnlyDictionary<uint, IAction> availableActions, Action<uint> selectAction);
    }
}