using EOAE_Code.Tests.Fixtures;
using NSubstitute;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Tests.StatusEffects
{
    public class AgentEffectsTests
    {
        [Fact]
        public void InstantiatesAgentEffects()
        {
            var mockAgent = Substitute.For<Agent>();
            var agentEffects = new AgentEffectsFixture(mockAgent);

            Assert.NotNull(agentEffects);
        }
    }
}
