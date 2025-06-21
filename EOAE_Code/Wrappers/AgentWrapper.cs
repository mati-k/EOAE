using EOAE_Code.Extensions;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Wrappers
{
    public class AgentWrapper : WrapperBase<AgentWrapper, Agent>
    {
        private Agent agent => UnwrappedObject;

        public virtual bool IsActive() => agent.IsActive();

        public virtual void UpdateAgentProperties() => agent.UpdateAgentProperties();

        public virtual Vec3 Position => agent.Position;

        public virtual bool IsFadingOut() => agent.IsFadingOut();

        public virtual void DealDamage(Agent caster, float damage) =>
            agent.DealDamage(caster, damage);

        // Remove it once wrapper is more propagated
        public Agent GetAgent()
        {
            return agent;
        }
    }
}
