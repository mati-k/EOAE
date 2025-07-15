using System;
using System.Collections.Generic;
using System.Linq;
using EOAE_Code.Interfaces;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.TwoDimension;

namespace EOAE_Code.Magic.Spells.BombardTargeting
{
    public abstract class BombardTargetingBase
    {
        private const float FRIENDLY_AGENT_PENALTY = -1.5f;
        protected const float VISION_ANGLE = 120 * Mathf.Deg2Rad;
        protected const float HALF_VISION_ANGLE = VISION_ANGLE / 2;

        public abstract MatrixFrame GetBestFrame(Agent caster, IUseAreaAim spell);

        protected static float GetScoreAtPosition(
            Vec2 position,
            Agent caster,
            List<Agent> agents,
            IUseAreaAim spell
        )
        {
            var spellRadiusSquared = Math.Pow(spell.Radius, 2);
            float score = 0f;

            foreach (var agent in agents)
            {
                if (agent.Position.AsVec2.DistanceSquared(position) <= spellRadiusSquared)
                {
                    score += agent.IsEnemyOf(caster) ? 1f : FRIENDLY_AGENT_PENALTY;
                }
            }

            return score;
        }

        protected static List<Agent> GetAgentsWithinVision(Agent caster, IUseAreaAim spell)
        {
            var spellDistanceSquared = Math.Pow(spell.Range + spell.Radius, 2);

            var humanAgents = Mission.Current.Agents.Where(agent => agent.IsHuman).ToList();
            return humanAgents
                .Where(agent =>
                    agent.Position.DistanceSquared(caster.Position) <= spellDistanceSquared
                )
                .Where(agent =>
                    Math.Abs(
                        caster.LookDirection.AsVec2.AngleBetween(
                            (agent.Position - caster.Position).AsVec2
                        )
                    ) <= HALF_VISION_ANGLE
                )
                .ToList();
        }

        public virtual bool QuickValidate(Agent caster, IUseAreaAim spell)
        {
            var validAgents = GetAgentsWithinVision(caster, spell);
            var hostileAgents = validAgents.Where(agent => agent.IsEnemyOf(caster));
            return hostileAgents.Any();
        }
    }
}
