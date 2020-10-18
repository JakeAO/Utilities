using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.GameState;
using SadPumpkin.Util.CombatEngine.StateChangeEvents;
using SadPumpkin.Util.Signals;

namespace SadPumpkin.Util.CombatEngine.Signals
{
    public class GameStateUpdated : Signal<IGameState, IReadOnlyList<IStateChangeEvent>>
    {
    }
}