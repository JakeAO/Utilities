using SadPumpkin.Util.CombatEngine.Actor;

namespace SadPumpkin.Util.CombatEngine.StateChangeEvents
{
    public class CharacterChangeEventBase : ActorChangeEventBase
    {
        public new ICharacterActor Actor { get; }

        public CharacterChangeEventBase(
            uint oldStateId, uint newStateId,
            ICharacterActor actor)
            : base(oldStateId, newStateId, actor)
        {
            Actor = actor;
        }
    }
}