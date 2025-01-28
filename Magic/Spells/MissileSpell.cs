using EOAE_Code.Data.Xml.Spells;
using EOAE_Code.Data.Xml.StatusEffects;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic.Spells
{
    public class MissileSpell : Spell
    {
        public StatusEffectBase? StatusEffect { get; private set; }
        public override bool IsThrown => true;

        public MissileSpell(SpellData data)
            : base(data) 
        {
            MissileSpellData missileSpellData = data as MissileSpellData;
            StatusEffect = missileSpellData.StatusEffect;
        }

        public override void Cast(Agent caster) { }

        // No restriction needed for regular thrown spells
        public override bool IsAICastValid(Agent caster)
        {
            return true;
        }
    }
}
