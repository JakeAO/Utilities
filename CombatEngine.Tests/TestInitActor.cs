using System;
using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.Tests
{
    public class TestInitActor : IInitiativeActor
    {
        public uint Id { get; set; }
        public uint Party { get; set; }
        public string Name { get; set; }

        public bool Alive { get; set; }
        public float Init { get; set; }

        public bool IsAlive() => Alive;
        public float GetInitiative() => Init;

        public IReadOnlyCollection<IAction> GetAllActions(IReadOnlyCollection<ITargetableActor> possibleTargets) => Array.Empty<IAction>();

        public IInitiativeActor Copy() => new TestInitActor
        {
            Id = Id,
            Party = Party,
            Name = Name,
            Alive = Alive,
            Init = Init
        };
    }
}