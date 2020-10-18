using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.GameState
{
    public interface IGameState
    {
        uint Id { get; }
        CombatState State { get; }
        IInitiativeActor ActiveActor { get; }
        IReadOnlyList<IInitiativePair> InitiativeOrder { get; }
    }
}