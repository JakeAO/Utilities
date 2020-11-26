﻿using System;
using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.EffectCalculators
{
    public class NoEffect : IEffectCalc
    {
        public static readonly NoEffect Instance = new NoEffect();

        public string Description { get; } = String.Empty;

        public void Apply(IInitiativeActor sourceEntity, IReadOnlyCollection<ITargetableActor> targetCharacters)
        {
            // Intentionally left blank
        }
    }
}