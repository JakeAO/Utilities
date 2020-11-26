using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.RequirementCalculators
{
    public interface IRequirementCalc
    {
        bool MeetsRequirement(IInitiativeActor actor);
    }
}