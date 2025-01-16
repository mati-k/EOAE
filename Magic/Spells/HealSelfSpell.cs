using System;
using EOAE_Code.Data.Xml;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic.Spells
{
    public class HealSelfSpell : Spell
    {
        public override bool IsThrown => false;

        public HealSelfSpell(SpellDataXml data)
            : base(data) { }

        public override void Cast(Agent caster)
        {
            caster.Health = Math.Min(caster.Health + EffectValue, caster.HealthLimit);
        }
    }
}
