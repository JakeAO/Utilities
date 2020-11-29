using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.RequirementCalculators
{
    public interface IRequirementCalc
    {
        string Description { get; }
        bool MeetsRequirement(IInitiativeActor actor);
    }
}