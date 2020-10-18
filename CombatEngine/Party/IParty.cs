using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.CharacterControllers;

namespace SadPumpkin.Util.CombatEngine.Party
{
    public interface IParty
    {
        uint Id { get; }
        ICharacterController Controller { get; }
        IReadOnlyCollection<IInitiativeActor> Actors { get; }
    }
}