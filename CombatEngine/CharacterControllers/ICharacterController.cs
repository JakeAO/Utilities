using System;
using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.CharacterControllers
{
    /// <summary>
    /// Interface which defines a handler for Actors' Actions.
    /// </summary>
    public interface ICharacterController
    {
        /// <summary>
        /// Prompt the controller to select an Action from the provided collection.
        /// </summary>
        /// <param name="activeEntity">Currently active Actor in the combat sequence.</param>
        /// <param name="availableActions">All available Actions for the Actor.</param>
        /// <param name="selectAction">Action to invoke once the Action has been selected, providing the Action's Id.</param>
        void SelectAction(IInitiativeActor activeEntity, IReadOnlyDictionary<uint, IAction> availableActions, Action<uint> selectAction);
    }
}