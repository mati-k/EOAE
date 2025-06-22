using EOAE_Code.StatusEffects;
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

            var multiplierProperties = StatusEffectMissionLogic.GetAgentStatModifiers(agent);
            if (multiplierProperties != null)
            {
                agent.AgentDrivenProperties.MaxSpeedMultiplier *=
                    multiplierProperties.MaxSpeedMultiplier;
            }
        }
    }
}
