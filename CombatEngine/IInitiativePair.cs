using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine
{
    public interface IInitiativePair : ICopyable<IInitiativePair>
    {
        IInitiativeActor Entity { get; }
        float Initiative { get; }
    }
}