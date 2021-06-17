using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.CharacterControllers;

namespace SadPumpkin.Util.CombatEngine.TurnController
{
    public class OneActionTurnController : ITurnController
    {
        private const int ACTION_REQUEST_SLEEP_TIME = 500;

        private uint? _pendingSelectedActionId = null;

        public IEnumerable<IAction> TakeTurn(
            IInitiativeActor activeActor,
            ICharacterController actorController,
            IReadOnlyCollection<ITargetableActor> allTargets,
            IStandardActionGenerator standardActionGenerator)
        {
            // Get active actor's action options
            Dictionary<uint, IAction> actionsForEntity = activeActor
                .GetAllActions(allTargets)
                .Concat(standardActionGenerator.GetActions(activeActor))
                .ToDictionary(x => x.Id);

            // Send options and wait for valid response
            yield return SendActionsAndWaitForResponse(
                activeActor,
                actorController,
                actionsForEntity);
        }

        private IAction SendActionsAndWaitForResponse(
            IInitiativeActor activeEntity,
            ICharacterController activeController,
            IReadOnlyDictionary<uint, IAction> availableActions)
        {
            IAction selectedAction;
            do
            {
                // Send Actions to Controller
                _pendingSelectedActionId = null;
                activeController.SelectAction(activeEntity, availableActions, OnActionSelected);

                // Wait for Controller response
                do
                {
                    Thread.Sleep(ACTION_REQUEST_SLEEP_TIME);
                } while (!_pendingSelectedActionId.HasValue);

                // Validate response
                availableActions.TryGetValue(_pendingSelectedActionId.Value, out selectedAction);

                _pendingSelectedActionId = null;

            } while (selectedAction == null || !selectedAction.Available);

            return selectedAction;
        }

        private void OnActionSelected(uint actionId)
        {
            _pendingSelectedActionId = actionId;
        }
    }
}