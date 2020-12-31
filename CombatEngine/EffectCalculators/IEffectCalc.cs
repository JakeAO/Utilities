using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.EffectCalculators
{
    /// <summary>
    /// Interface defining the Effect of an Action.
    /// </summary>
    public interface IEffectCalc
    {
        /// <summary>
        /// User-readable description of the Effect.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Apply this Effect to the target Actors provided.
        /// </summary>
        /// <param name="sourceEntity">Actor which originates this Effect.</param>
        /// <param name="targetActors">Actors which receive this Effect.</param>
        void Apply(IInitiativeActor sourceEntity, IReadOnlyCollection<ITargetableActor> targetActors);
    }
}