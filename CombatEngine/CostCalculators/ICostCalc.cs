using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.CostCalculators
{
    public interface ICostCalc
    {
        bool CanAfford(IInitiativeActor entity, IIdTracked actionSource);
        void Pay(IInitiativeActor entity, IIdTracked actionSource);
        string Description();
    }
}