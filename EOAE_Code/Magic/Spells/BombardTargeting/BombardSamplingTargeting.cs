using EOAE_Code.Agents;
using EOAE_Code.Interfaces;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic.Spells.BombardTargeting
{
    // Complexity GRID_SIZE * GRID_SIZE * Agents
    public class BombardSamplingTargeting : BombardTargetingBase
    {
        private const int GRID_SIZE = 15;

        private static BombardSamplingTargeting? instance;
        public static BombardSamplingTargeting Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BombardSamplingTargeting();
                }
                return instance;
            }
        }

        public override MatrixFrame GetBestFrame(Agent caster, IUseAreaAim spell)
        {
            var validAgents = GetAgentsWithinVision(caster, spell);

            var bestFrame = Vec2.Zero;
            var bestScore = 0f;
            for (int x = 0; x < GRID_SIZE; x++)
            {
                var rotation = (x + 0.5f - GRID_SIZE / 2);
                var direction = caster.LookDirection.AsVec2.Normalized();
                direction.RotateCCW(rotation);

                for (int y = 1; y < GRID_SIZE + 1; y++)
                {
                    var spellCenter =
                        caster.Position.AsVec2 + direction * spell.Range * y / GRID_SIZE;
                    var score = GetScoreAtPosition(spellCenter, caster, validAgents, spell);

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestFrame = spellCenter;
                    }
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
