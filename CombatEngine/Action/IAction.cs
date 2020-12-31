using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.CostCalculators;
using SadPumpkin.Util.CombatEngine.EffectCalculators;

namespace SadPumpkin.Util.CombatEngine.Action
{
    /// <summary>
    /// Action presented to a CharacterController during an Actor's turn.
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// Unique identifier for the Action.
        /// </summary>
        uint Id { get; }

        /// <summary>
        /// Action currently available to the Actor.
        /// </summary>
        bool Available { get; }

        /// <summary>
        /// Actor which will initiate this Action.
        /// </summary>
        IInitiativeActor Source { get; }

        /// <summary>
        /// Actors which will receive the Effect of this Action.
        /// </summary>
        IReadOnlyCollection<ITargetableActor> Targets { get; }

        /// <summary>
        /// Initiative cost of this Action.
        /// </summary>
        uint Speed { get; }

        /// <summary>
        /// Additional Cost that this Action requires.
        /// </summary>
        ICostCalc Cost { get; }

        /// <summary>
        /// Effect this Action will apply to the target Actors.
        /// </summary>
        IEffectCalc Effect { get; }

        /// <summary>
        /// The trackable object which directly created this Action.
        /// </summary>
        IIdTracked ActionSource { get; }

        /// <summary>
        /// The trackable object which acts as the root context of the Action.
        /// </summary>
        IIdTracked ActionProvider { get; }
    }
}