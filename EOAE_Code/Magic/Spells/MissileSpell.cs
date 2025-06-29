using EOAE_Code.Data.Xml.Spells;
using EOAE_Code.Data.Xml.StatusEffects;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic.Spells
{
    public class MissileSpell : Spell
    {
        public int ExtraMissiles { get; private set; }
        public float MissileSpread { get; private set; }
        public StatusEffect? Effect { get; private set; }
        public override bool IsThrown => true;

        public MissileSpell(SpellData data)
            : base(data)
        {
            MissileSpellData missileSpellData = data as MissileSpellData;

            ExtraMissiles = missileSpellData.ExtraMissiles;
            MissileSpread = missileSpellData.MissileSpread;
            Effect = missileSpellData.Effect;
        }

        public override void Cast(Agent caster) { }

        // No restriction needed for regular thrown spells
        public override bool IsAICastValid(Agent caster)
        {
            return true;
        }
    }
}
