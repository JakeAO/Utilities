namespace SadPumpkin.Util.CombatEngine.Actor
{
    public interface ITargetableActor : IInitiativeActor
    {
        bool CanTarget();
    }
}