using EOAE_Code.Data.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic.Spells
{
    public class HealGroupSpell : Spell
    {
        public float HealEffect {  get; private set; }
        public float HealRadius { get; private set; }
        public override bool IsThrown { get { return false; } }

        public HealGroupSpell(SpellDataXml data) : base(data)
        {
            HealEffect = data.EffecValue;
            HealRadius = data.Range;
        }

        public override void Cast(Agent caster)
        {
            MBList<Agent> agents = new MBList<Agent>(Mission.Current.Agents);
            Mission.Current.GetNearbyAllyAgents(caster.Position.AsVec2, HealRadius, caster.Team, agents);

            foreach (Agent agent in agents)
            {
                agent.Health = Math.Min(agent.Health + HealEffect, agent.HealthLimit);
            }
        }
    }
}
