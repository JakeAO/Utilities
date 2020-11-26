using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.CostCalculators
{
    public class NoCost : ICostCalc
    {
        public static readonly NoCost Instance = new NoCost();
        
        public bool CanAfford(IInitiativeActor entity, IIdTracked actionSource)
        {
            return true;
        }

        public void Pay(IInitiativeActor entity, IIdTracked actionSource)
        {
            // Intentionally left blank
        }

        public string Description()
        {
            return string.Empty;
        }
    }
}