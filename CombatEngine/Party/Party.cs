using System.Collections.Generic;
using SadPumpkin.Util.CombatEngine.Actor;
using SadPumpkin.Util.CombatEngine.CharacterControllers;

namespace SadPumpkin.Util.CombatEngine.Party
{
    public class Party : IParty
    {
        public uint Id { get; }
        public ICharacterController Controller { get; }
        public IReadOnlyCollection<IInitiativeActor> Actors { get; }

        public Party(
            uint id,
            ICharacterController controller,
            IReadOnlyCollection<ICharacterActor> characters)
        {
            Id = id;
            Controller = controller;
            Actors = new List<IInitiativeActor>(characters);
        }
    }
}