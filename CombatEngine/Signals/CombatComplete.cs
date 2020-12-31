using SadPumpkin.Util.Signals;

namespace SadPumpkin.Util.CombatEngine.Signals
{
    /// <summary>
    /// Signal which will be fired when combat is finished,
    /// the value of which is the Id of the winning Party.
    /// </summary>
    public class CombatComplete : Signal<uint>
    {
        
    }
}