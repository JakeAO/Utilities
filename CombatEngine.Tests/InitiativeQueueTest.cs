using NUnit.Framework;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.Initiatives;

namespace SadPumpkin.Util.CombatEngine.Tests
{
    [TestFixture]
    public class InitiativeQueueTest
    {
        [Test]
        public void max_init_never_exceeds_threshold_by_much()
        {
            IInitiativeQueue queue = new InitiativeQueue(
                100,
                new[]
                {
                    new TestInitActor() {Init = 10},
                    new TestInitActor() {Init = 10},
                    new TestInitActor() {Init = 10},
                    new TestInitActor() {Init = 10},
                    new TestInitActor() {Init = 10}
                });

            float initLimit = 115;
            for (int i = 0; i < 1000; i++)
            {
                foreach ((uint _, float initiative) in queue.GetCurrentQueue())
                {
                    Assert.Less(initiative, initLimit);
                }

                IInitiativeActor nextActor = queue.GetNext();
                queue.Update(nextActor.Id, 100);
            }
        }
        
        [Test]
        public void actors_take_turns_in_order()
        {
            var actors = new (IInitiativeActor actor, float initialValue)[]
            {
                (new TestInitActor() {Init = 10, Id = 0}, 4f),
                (new TestInitActor() {Init = 10, Id = 1}, 3f),
                (new TestInitActor() {Init = 10, Id = 2}, 2f),
                (new TestInitActor() {Init = 10, Id = 3}, 1f),
                (new TestInitActor() {Init = 10, Id = 4}, 0f)
            };
            IInitiativeQueue queue = new InitiativeQueue(100, actors);

            for (int i = 0; i < 1000; i++)
            {
                IInitiativeActor expectedActor = actors[i % 5].actor;
                IInitiativeActor nextActor = queue.GetNext();
                queue.Update(nextActor.Id, 100);

                Assert.AreEqual(expectedActor.Id, nextActor.Id);
            }
        }
    }
}