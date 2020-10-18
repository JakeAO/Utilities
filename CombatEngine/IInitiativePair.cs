using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine
{
    public interface IInitiativePair
    {
        IInitiativeActor Entity { get; }
        float Initiative { get; }
    }
}