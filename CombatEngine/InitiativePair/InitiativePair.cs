using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine
{
    public class InitiativePair : IInitiativePair, IWritableInitiativePair
    {
        public IInitiativeActor Entity { get; }
        public float Initiative { get; set; }
        
        public InitiativePair(IInitiativeActor entity, float init)
        {
            Entity = entity;
            Initiative = init;
        }

        public void IncrementInitiative(float increment) => Initiative += increment;

        public IInitiativePair Copy()
        {
            return new InitiativePair(
                Entity.Copy(),
                Initiative);
        }
    }
}