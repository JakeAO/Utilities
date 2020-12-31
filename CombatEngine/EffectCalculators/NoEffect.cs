using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.EffectCalculators
{
    /// <summary>
    /// Implementation of Effect calculator with no Effect.
    /// </summary>
    public class NoEffect : IEffectCalc
    {
        public static readonly NoEffect Instance = new NoEffect();

        /// <summary>
        /// User-readable description of the Effect.
        /// </summary>
        public string Description => "None";

        /// <summary>
        /// Apply this Effect to the target Actors provided.
        /// </summary>
        /// <param name="sourceEntity">Actor which originates this Effect.</param>
        /// <param name="targetActors">Actors which receive this Effect.</param>
        public void Apply(IInitiativeActor sourceEntity, IReadOnlyCollection<ITargetableActor> targetActors)
        {
            // Intentionally left blank
        }
    }
}