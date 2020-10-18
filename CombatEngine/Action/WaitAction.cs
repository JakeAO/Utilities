using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Abilities;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.CostCalculators;
using SadPumpkin.Util.CombatEngine.EffectCalculators;
using SadPumpkin.Util.CombatEngine.RequirementCalculators;
using SadPumpkin.Util.CombatEngine.TargetCalculators;

namespace SadPumpkin.Util.CombatEngine.Action
{
    public class WaitAction : IAction
    {
        public class WaitAbility : IAbility
        {
            public static readonly WaitAbility Instance = new WaitAbility();

            public uint Id { get; } = 0;
            public string Name => "Wait";
            public string Desc => "Delay your turn.";
            public uint Speed => 50;
            public IRequirementCalc Requirements => NoRequirements.Instance;
            public ICostCalc Cost => NoCost.Instance;
            public ITargetCalc Target => SelfTargetCalculator.Instance;
            public IEffectCalc Effect => NoEffect.Instance;
        }

        public uint Id { get; } = ActionUtil.NextId;
        public bool Available => true;
        public IAbility Ability => WaitAbility.Instance;
        public IInitiativeActor Source { get; }
        public IReadOnlyCollection<ICharacterActor> Targets { get; }

        public WaitAction(ICharacterActor sourceCharacter)
        {
            Source = sourceCharacter;
            Targets = new[] {sourceCharacter};
        }
    }
}