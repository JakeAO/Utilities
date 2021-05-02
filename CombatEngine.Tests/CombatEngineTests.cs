using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using SadPumpkin.Util.CombatEngine.Action;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.ActorChangeCalculator;
using SadPumpkin.Util.CombatEngine.CharacterControllers;
using SadPumpkin.Util.CombatEngine.CostCalculators;
using SadPumpkin.Util.CombatEngine.EffectCalculators;
using SadPumpkin.Util.CombatEngine.Initiatives;
using SadPumpkin.Util.CombatEngine.Party;
using SadPumpkin.Util.CombatEngine.WinningPartyCalculator;
using SadPumpkin.Util.Events;

namespace SadPumpkin.Util.CombatEngine.Tests
{
    [TestFixture]
    public class CombatEngineTests
    {
        private class TestParty : IParty
        {
            public uint Id { get; set; } = 0u;
            public ICharacterController Controller { get; set; } = new RandomCharacterController();
            public IReadOnlyCollection<IInitiativeActor> Actors { get; set; } = new List<IInitiativeActor>();
        }

        private class TestAction : IAction
        {
            public uint Id { get; set; } = 0u;
            public bool Available { get; set; } = true;
            public IInitiativeActor Source { get; set; } = null;
            public IReadOnlyCollection<ITargetableActor> Targets { get; set; } = new ITargetableActor[0];
            public uint Speed { get; set; } = 100;
            public ICostCalc Cost { get; set; } = new NoCost();
            public IEffectCalc Effect { get; set; } = new NoEffect();
            public IIdTracked ActionSource { get; set; } = null;
            public IIdTracked ActionProvider { get; set; } = null;
        }

        private class TestActor : IInitiativeActor, ITargetableActor
        {
            public IInitiativeActor Copy()
            {
                return new TestActor()
                {
                    Id = this.Id,
                    Party = this.Party,
                    Name = this.Name
                };
            }

            public uint Id { get; set; }
            public uint Party { get; set; }
            public string Name { get; set; }
            public bool IsAlive() => true;
            public float GetInitiative() => 10;
            public bool CanTarget() => true;

            public IReadOnlyCollection<IAction> GetAllActions(IReadOnlyCollection<ITargetableActor> possibleTargets)
            {
                return new IAction[]
                {
                    new TestAction()
                    {
                        Source = this,
                        Targets = new ITargetableActor[] {this}
                    }
                };
            }
        }

        [Test]
        public void can_create()
        {
            CombatManager combatManager = new CombatManager(
                new IParty[]
                {
                    new TestParty()
                    {
                        Id = 1u,
                        Actors = new IInitiativeActor[]
                        {
                            new TestActor()
                            {
                                Id = 10,
                                Name = "Geoff",
                                Party = 1u
                            },
                        }
                    },
                    new TestParty()
                    {
                        Id = 2u,
                        Actors = new IInitiativeActor[]
                        {
                            new TestActor()
                            {
                                Id = 20,
                                Name = "Jeff",
                                Party = 2u
                            },
                        }
                    },
                },
                new NullStandardActionGenerator(),
                new AnyAliveWinningPartyCalculator(),
                new NullActorChangeCalculator(),
                new InitiativeQueue(100),
                new EventQueue());

            Assert.IsNotNull(combatManager);
            Assert.IsInstanceOf<CombatManager>(combatManager);
        }

        [Test]
        public void can_start_manual_thread()
        {
            CombatManager combatManager = new CombatManager(
                new IParty[]
                {
                    new TestParty()
                    {
                        Id = 1u,
                        Actors = new IInitiativeActor[]
                        {
                            new TestActor()
                            {
                                Id = 10,
                                Name = "Geoff",
                                Party = 1u
                            },
                        }
                    },
                    new TestParty()
                    {
                        Id = 2u,
                        Actors = new IInitiativeActor[]
                        {
                            new TestActor()
                            {
                                Id = 20,
                                Name = "Jeff",
                                Party = 2u
                            },
                        }
                    },
                },
                new NullStandardActionGenerator(),
                new AnyAliveWinningPartyCalculator(),
                new NullActorChangeCalculator(),
                new InitiativeQueue(100),
                new EventQueue());

            Task.Run(() => combatManager.Start(false));

            Assert.Pass();
        }

        [Test]
        public void can_start_auto_thread()
        {
            CombatManager combatManager = new CombatManager(
                new IParty[]
                {
                    new TestParty()
                    {
                        Id = 1u,
                        Actors = new IInitiativeActor[]
                        {
                            new TestActor()
                            {
                                Id = 10,
                                Name = "Geoff",
                                Party = 1u
                            },
                        }
                    },
                    new TestParty()
                    {
                        Id = 2u,
                        Actors = new IInitiativeActor[]
                        {
                            new TestActor()
                            {
                                Id = 20,
                                Name = "Jeff",
                                Party = 2u
                            },
                        }
                    },
                },
                new NullStandardActionGenerator(),
                new AnyAliveWinningPartyCalculator(),
                new NullActorChangeCalculator(),
                new InitiativeQueue(100),
                new EventQueue());

            combatManager.Start(true);

            Assert.Pass();
        }
    }
}