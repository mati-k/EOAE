using EOAE_Code.Magic.StatusEffect;
using SandBox.GameComponents;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Models
{
    public class AgentStatusEffectModel : SandboxAgentStatCalculateModel
    {
        public override void UpdateAgentStats(
            Agent agent,
            AgentDrivenProperties agentDrivenProperties
        )
        {
            base.UpdateAgentStats(agent, agentDrivenProperties);

            var multiplierProperties = EffectMissionLogic.GetAgentStatModifiers(agent);
            if (multiplierProperties != null)
            {
                agent.AgentDrivenProperties.MaxSpeedMultiplier *=
                    multiplierProperties.MaxSpeedMultiplier;
            }
        }
    }
}
