using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.RequirementCalculators
{
    public class NoRequirements : IRequirementCalc
    {
        public static readonly NoRequirements Instance = new NoRequirements();

        public string Description => "None";
        
        public bool MeetsRequirement(IInitiativeActor actor)
        {
            return true;
        }
    }
}