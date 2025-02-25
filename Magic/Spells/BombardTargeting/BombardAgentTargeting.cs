using EOAE_Code.Agents;
using EOAE_Code.Interfaces;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic.Spells.BombardTargeting
{
    // Complexity Agents ^ 2
    public class BombardAgentTargeting : BombardTargetingBase
    {
        private static BombardAgentTargeting? instance;
        public static BombardAgentTargeting Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BombardAgentTargeting();
                }
                return instance;
            }
        }

        public override MatrixFrame GetBestFrame(Agent caster, IUseAreaAim spell)
        {
            var validAgents = GetAgentsWithinVision(caster, spell);

            var bestFrame = Vec2.Zero;
            var bestScore = 0f;

            // Including allies as starting point, in case of situation like single surrounded ally in the middle
            foreach (var sourceAgent in validAgents)
            {
                var spellCenter = sourceAgent.Position.AsVec2;
                var score = GetScoreAtPosition(spellCenter, caster, validAgents, spell);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestFrame = spellCenter;
                }
            }

            if (bestFrame == Vec2.Zero)
            {
                return MatrixFrame.Zero;
            }

            return new MatrixFrame(
                Mat3.Identity,
                new Vec3(bestFrame, MagicAgentUtils.GetHeightAtPoint(bestFrame, spell))
            );
        }
    }
}
