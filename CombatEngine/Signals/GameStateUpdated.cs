using SadPumpkin.Util.CombatEngine.GameState;
using SadPumpkin.Util.Signals;

namespace SadPumpkin.Util.CombatEngine.Signals
{
    /// <summary>
    /// Signal which will be fired when the combat state is updated,
    /// the value of which is a copy of the current GameState.
    /// </summary>
    public class GameStateUpdated : Signal<IGameState>
    {
    }
}