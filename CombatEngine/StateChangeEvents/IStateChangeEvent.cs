namespace SadPumpkin.Util.CombatEngine.StateChangeEvents
{
    public interface IStateChangeEvent
    {
        uint OldStateId { get; }
        uint NewStateId { get; }
        string Description { get; }
    }
}