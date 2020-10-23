using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.CostCalculators
{
    public interface ICostCalc
    {
        bool CanAfford(IInitiativeActor entity);
        void Pay(IInitiativeActor entity);
        string Description();
    }
}