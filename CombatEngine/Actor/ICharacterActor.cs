using SadPumpkin.Util.CombatEngine.CharacterClasses;
using SadPumpkin.Util.CombatEngine.Item.Weapons;
using SadPumpkin.Util.CombatEngine.StatMap;

namespace SadPumpkin.Util.CombatEngine.Actor
{
    public interface ICharacterActor : ITargetableActor
    {
        ICharacterClass Class { get; }
        IStatMap Stats { get; }
        
        float GetReducedDamage(float damageAmount, DamageType damageType);
    }
}