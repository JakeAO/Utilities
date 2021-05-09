namespace SadPumpkin.Util.UXEventQueue
{
    public interface IUpdateUXEvent : IUXEvent
    {
        void TickUpdate(float deltaTimeMs);
    }
}