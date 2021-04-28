namespace SadPumpkin.Util.CombatEngine
{
    public interface IWritableInitiativePair : IInitiativePair
    {
        void IncrementInitiative(float increment);
    }
}