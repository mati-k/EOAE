using EOAE_Code.Tests.Fixtures;
using EOAE_Code.Wrappers;
using NSubstitute;

namespace EOAE_Code.Tests.StatusEffects
{
    public class AgentEffectsTests
    {
        [Fact]
        public void InstantiatesAgentEffects()
        {
            var mockAgent = Substitute.For<AgentWrapper>();
            var agentEffects = new AgentEffectsFixture(mockAgent);

            Assert.NotNull(agentEffects);
        }
    }
}
