using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine
{
    public class InitiativePair : IInitiativePair
    {
        public IInitiativeActor Entity { get; }
        public float Initiative { get; set; }

        public InitiativePair(IInitiativeActor entity, float init)
        {
            Entity = entity;
            Initiative = init;
        }
    }
}