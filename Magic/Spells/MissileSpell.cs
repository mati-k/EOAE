using EOAE_Code.Data.Xml;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic.Spells
{
    public class MissileSpell : Spell
    {
        public override bool IsThrown => true;

        public MissileSpell(SpellDataXml data)
            : base(data) { }

        public override void Cast(Agent caster) { }

        // No restriction needed for regular thrown spells
        public override bool IsAICastValid(Agent caster)
        {
            return true;
        }
    }
}
