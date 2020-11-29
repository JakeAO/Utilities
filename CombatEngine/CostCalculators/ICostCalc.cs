using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.CostCalculators
{
    public interface ICostCalc
    {
        string Description { get; }
        bool CanAfford(IInitiativeActor entity, IIdTracked actionSource);
        void Pay(IInitiativeActor entity, IIdTracked actionSource);
    }
}