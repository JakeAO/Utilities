using System;
using System.Collections.Generic;
using System.Linq;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.CharacterControllers
{
    /// <summary>
    /// Implementation of CharacterController which selects actions randomly.
    /// </summary>
    public class RandomCharacterController : ICharacterController
    {
        private static readonly Random RANDOM = new Random();

        /// <summary>
        /// Prompt the controller to select an Action from the provided collection randomly.
        /// </summary>
        /// <param name="activeEntity">Currently active Actor in the combat sequence.</param>
        /// <param name="availableActions">All available Actions for the Actor.</param>
        /// <param name="selectAction">Action to invoke once the Action has been selected, providing the Action's Id.</param>
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