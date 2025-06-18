using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Wrappers
{
    public class AgentWrapper
    {
        private Agent agent;

        public AgentWrapper() { }

        public AgentWrapper(Agent agent)
        {
            this.agent = agent;
        }

        public bool IsActive() => agent.IsActive();

        public void UpdateAgentProperties() => agent.UpdateAgentProperties();

        public Vec3 Position => agent.Position;

        // Remove it once wrapper is more propagated
        public Agent GetAgent()
        {
            return agent;
        }
    }
}
