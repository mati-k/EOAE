using System;
using EOAE_Code.Data.Xml;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic.Spells
{
    public class HealGroupSpell : Spell
    {
        public override bool IsThrown => false;

        public HealGroupSpell(SpellDataXml data)
            : base(data) { }

        public override void Cast(Agent caster)
        {
            var agents = new MBList<Agent>(Mission.Current.Agents);
            Mission.Current.GetNearbyAllyAgents(
                caster.Position.AsVec2,
                AreaRange,
                caster.Team,
                agents
            );

            foreach (var agent in agents)
            {
                agent.Health = Math.Min(agent.Health + EffectValue, agent.HealthLimit);
            }
        }
    }
}
