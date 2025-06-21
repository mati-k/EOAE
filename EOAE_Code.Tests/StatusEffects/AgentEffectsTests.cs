using EOAE_Code.Data.Xml.StatusEffects;
using EOAE_Code.Magic.StatusEffect;
using EOAE_Code.Tests.Fixtures;
using EOAE_Code.Wrappers;
using NSubstitute;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Tests.StatusEffects
{
    public class AgentEffectsTests
    {
        AgentWrapper mockAgent = Substitute.For<AgentWrapper>();
        Effect sampleEffect = new Effect
        {
            Duration = 15.0f,
            Actions = new List<EffectAction>()
            {
                new DamageOverTimeEffectData { Value = 15.0f },
                new DamageOverTimeEffectData { Value = 10.0f },
                new DamageOverTimeEffectData { Value = 5.0f, Key = "fire_damage" },
                new DamageOverTimeEffectData { Value = 9.0f, Key = "poison_damage" },
                new MovementSpeedEffectData { Value = -0.5f },
                new MovementSpeedEffectData { Value = -0.1f },
                new MovementSpeedEffectData { Value = -0.3f, Key = "freeze_speed" },
                new MovementSpeedEffectData { Value = -0.2f, Key = "poison_speed" },
            },
        };

        public AgentEffectsTests()
        {
            mockAgent.IsActive().ReturnsForAnyArgs(true);
            mockAgent.IsFadingOut().ReturnsForAnyArgs(false);
        }

        [Fact]
        public void InstantiatesAgentEffects()
        {
            var agentEffects = new AgentEffectsFixture(mockAgent);

            Assert.NotNull(agentEffects);
        }

        [Fact]
        public void AddStatusEffect_AddsEffectToActiveEffects()
        {
            var agentEffects = new AgentEffectsFixture(mockAgent);
            var appliedEffect = new AppliedEffect(sampleEffect, null);

            agentEffects.AddStatusEffect(appliedEffect);
            Assert.Contains(appliedEffect, agentEffects.ActiveEffects);
        }

        [Fact]
        public void AddStatusEffect_AddsModifiersToExclusiveAndStackable()
        {
            var agentEffects = new AgentEffectsFixture(mockAgent);
            var appliedEffect = new AppliedEffect(sampleEffect, null);
            agentEffects.AddStatusEffect(appliedEffect);

            // Check exclusive modifiers
            foreach (var action in sampleEffect.Actions)
            {
                if (action is Modifier modifier)
                {
                    if (String.IsNullOrEmpty(modifier.Key))
                    {
                        Assert.Contains(modifier, agentEffects.StackableModifiers);
                    }
                    else
                    {
                        Assert.True(agentEffects.ExclusiveModifiers.ContainsKey(modifier.Key));
                        Assert.Contains(
                            new KeyValuePair<float, Modifier>(Math.Abs(modifier.Value), modifier),
                            agentEffects.ExclusiveModifiers[modifier.Key]
                        );
                    }
                }
            }
        }

        [Fact]
        public void AppliedEffect_Removed_After_Duration_Ends()
        {
            var agentEffects = new AgentEffectsFixture(mockAgent);
            var appliedEffect = new AppliedEffect(sampleEffect, null);
            agentEffects.AddStatusEffect(appliedEffect);

            Assert.Contains(appliedEffect, agentEffects.ActiveEffects);

            float dt = 0.2f;
            for (float timer = 0; timer < sampleEffect.Duration; timer += dt)
            {
                agentEffects.Tick(dt);
            }

            Assert.DoesNotContain(appliedEffect, agentEffects.ActiveEffects);
        }

        [Fact]
        public void FireEffectTick_FiresPulseEffect_AfterTickRate()
        {
            var agentEffects = new AgentEffectsFixture(mockAgent);

            var mockModifier = Substitute.For<Modifier>();

            var effect = new Effect
            {
                Duration = 5.0f,
                Actions = new List<EffectAction> { mockModifier },
            };
            var appliedEffect = new AppliedEffect(effect, null);

            agentEffects.AddStatusEffect(appliedEffect);

            float tickStep = agentEffects.TickRate * 0.4f;

            // Tick few times before reaching pulse
            agentEffects.Tick(tickStep);
            mockModifier
                .DidNotReceive()
                .Tick(Arg.Any<float>(), Arg.Any<AgentWrapper>(), Arg.Any<Agent>());

            agentEffects.Tick(tickStep);
            mockModifier
                .DidNotReceive()
                .Tick(Arg.Any<float>(), Arg.Any<AgentWrapper>(), Arg.Any<Agent>());

            // Assert: Tick should have been called once
            agentEffects.Tick(tickStep);
            mockModifier
                .Received(1)
                .Tick(Arg.Any<float>(), Arg.Any<AgentWrapper>(), Arg.Any<Agent>());
        }
    }
}
