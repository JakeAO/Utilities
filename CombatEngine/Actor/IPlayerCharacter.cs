using SadPumpkin.Util.CombatEngine.EquipMap;

namespace SadPumpkin.Util.CombatEngine.Actor
{
    public interface IPlayerCharacterActor : ICharacterActor
    {
        IEquipMap Equipment { get; }
    }
}