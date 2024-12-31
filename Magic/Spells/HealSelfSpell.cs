using EOAE_Code.Data.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic.Spells
{
    public class HealSelfSpell : Spell
    {
        public float HealEffect {  get; private set; }
        public override bool IsThrown { get { return false; } }

        public HealSelfSpell(SpellDataXml data) : base(data)
        {
            HealEffect = data.EffecValue;
        }

        public override void Cast(Agent caster)
        {
            caster.Health += Math.Min(caster.Health + HealEffect, caster.HealthLimit);
        }
    }
}
