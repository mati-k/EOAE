using EOAE_Code.Data.Xml.StatusEffects;
using EOAE_Code.Magic.StatusEffect;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Tests.Fixtures
{
    public class AgentEffectsFixture : AgentEffects
    {
        public AgentEffectsFixture(Agent agent)
            : base(agent) { }

        public List<AppliedEffect> ActiveEffects => activeEffects;
        public Dictionary<
            string,
            TaleWorlds.Library.PriorityQueue<float, Modifier>
        > ExclusiveModifiers => exclusiveModifiers;
        public List<Modifier> StackableModifiers => stackableModifiers;
        public Dictionary<string, GameEntity> ParticleEffects => particleEffects;
    }
}
