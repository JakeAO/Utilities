using System;
using System.Collections.Generic;
using System.Linq;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.CharacterControllers
{
    public class RandomCharacterController : ICharacterController
    {
        private static readonly Random RANDOM = new Random();

        public void SelectAction(IInitiativeActor activeEntity, IReadOnlyDictionary<uint, IAction> availableActions, Action<uint> selectAction)
        {
            uint[] actionIds = availableActions
                .Where(x => x.Value.Available)
                .Select(x => x.Key)
                .ToArray();
            uint actionId = actionIds[RANDOM.Next(actionIds.Length)];

            selectAction(actionId);
        }
    }
}