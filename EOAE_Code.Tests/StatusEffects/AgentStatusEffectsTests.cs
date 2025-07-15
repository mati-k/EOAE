using EOAE_Code.Data.Xml.StatusEffects;
using EOAE_Code.StatusEffects;
using EOAE_Code.Tests.Fixtures;
using EOAE_Code.Wrappers;
using NSubstitute;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Tests.StatusEffects
{
    public class AgentStatusEffectsTests
    {
        AgentWrapper mockAgent = Substitute.For<AgentWrapper>();
        StatusEffect sampleEffect = new StatusEffect
        {
            Duration = 15.0f,
            Actions = new List<StatusEffectAction>()
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

        public AgentStatusEffectsTests()
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
            var appliedEffect = new AppliedStatusEffect(sampleEffect, null);

            agentEffects.AddStatusEffect(appliedEffect);
            Assert.Contains(appliedEffect, agentEffects.ActiveEffects);
        }

        [Fact]
        public void AddStatusEffect_AddsModifiersToExclusiveAndStackable()
        {
            var agentEffects = new AgentEffectsFixture(mockAgent);
            var appliedEffect = new AppliedStatusEffect(sampleEffect, null);
            agentEffects.AddStatusEffect(appliedEffect);

            // Check exclusive modifiers
            foreach (var action in sampleEffect.Actions)
            {
                if (action is Modifier modifier)
                {
                    var appliedModifier = new AppliedModifier(modifier, appliedEffect.Caster);

                    if (String.IsNullOrEmpty(modifier.Key))
                    {
                        Assert.Contains(appliedModifier, agentEffects.StackableModifiers);
                    }
                    else
                    {
                        Assert.True(agentEffects.ExclusiveModifiers.ContainsKey(modifier.Key));
                        Assert.Contains(
                            new KeyValuePair<float, AppliedModifier>(
                                Math.Abs(modifier.Value),
                                appliedModifier
                            ),
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
            var appliedEffect = new AppliedStatusEffect(sampleEffect, null);
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

            var effect = new StatusEffect
            {
                Duration = 5.0f,
                Actions = new List<StatusEffectAction> { mockModifier },
            };
            var appliedEffect = new AppliedStatusEffect(effect, null);

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

        [Fact]
        public void ExclusiveModifier_OnlyTopValueIsApplied()
        {
            var agentEffects = new AgentEffectsFixture(mockAgent);

            var modifierLow = Substitute.For<Modifier>();
            modifierLow.Key = "test_key";
            modifierLow.Value = 1.0f;

            var modifierMedium = Substitute.For<Modifier>();
            modifierMedium.Key = "test_key";
            modifierMedium.Value = 3.0f;

            var modifierHigh = Substitute.For<Modifier>();
            modifierHigh.Key = "test_key";
            modifierHigh.Value = 5.0f;

            void AddModifier(Modifier modifier)
            {
                var effect = new StatusEffect
                {
                    Duration = 5.0f,
                    Actions = new List<StatusEffectAction> { modifier },
                };
                agentEffects.AddStatusEffect(new AppliedStatusEffect(effect, null));
            }

            AddModifier(modifierLow);
            AddModifier(modifierHigh);
            AddModifier(modifierMedium);

            // Only the highest value modifier should be applied
            agentEffects.Tick(1.0f);
            modifierHigh.Received(1).Apply(Arg.Any<float>(), Arg.Any<AgentDrivenProperties>());
            modifierMedium
                .DidNotReceive()
                .Apply(Arg.Any<float>(), Arg.Any<AgentDrivenProperties>());
            modifierLow.DidNotReceive().Apply(Arg.Any<float>(), Arg.Any<AgentDrivenProperties>());
        }

        [Fact]
        public void ExclusiveModifier_NegativeValues_AreHandledCorrectly()
        {
            var agentEffects = new AgentEffectsFixture(mockAgent);

            var modifierLow = Substitute.For<Modifier>();
            modifierLow.Key = "test_key";
            modifierLow.Value = -1.0f;

            var modifierMedium = Substitute.For<Modifier>();
            modifierMedium.Key = "test_key";
            modifierMedium.Value = -3.0f;

            var modifierHigh = Substitute.For<Modifier>();
            modifierHigh.Key = "test_key";
            modifierHigh.Value = -5.0f;

            void AddModifier(Modifier modifier)
            {
                var effect = new StatusEffect
                {
                    Duration = 5.0f,
                    Actions = new List<StatusEffectAction> { modifier },
                };
                agentEffects.AddStatusEffect(new AppliedStatusEffect(effect, null));
            }

            AddModifier(modifierLow);
            AddModifier(modifierHigh);
            AddModifier(modifierMedium);

            // Only the highest value modifier should be applied
            agentEffects.Tick(1.0f);
            modifierHigh.Received(1).Apply(Arg.Any<float>(), Arg.Any<AgentDrivenProperties>());
            modifierMedium
                .DidNotReceive()
                .Apply(Arg.Any<float>(), Arg.Any<AgentDrivenProperties>());
            modifierLow.DidNotReceive().Apply(Arg.Any<float>(), Arg.Any<AgentDrivenProperties>());
        }

        [Fact]
        public void ExclusiveModifier_QueueUpdates_WhenEffectExpires()
        {
            var agentEffects = new AgentEffectsFixture(mockAgent);

            // Two exclusive modifiers, different durations
            var modShort = Substitute.For<Modifier>();
            modShort.Key = "expire_key";
            modShort.Value = 10.0f;

            var modLong = Substitute.For<Modifier>();
            modLong.Key = "expire_key";
            modLong.Value = 5.0f;

            var effectShort = new StatusEffect
            {
                Duration = agentEffects.TickRate * 1.5f,
                Actions = new List<StatusEffectAction> { modShort },
            };
            var effectLong = new StatusEffect
            {
                Duration = agentEffects.TickRate * 5f,
                Actions = new List<StatusEffectAction> { modLong },
            };

            var appliedShort = new AppliedStatusEffect(effectShort, null);
            var appliedLong = new AppliedStatusEffect(effectLong, null);

            agentEffects.AddStatusEffect(appliedShort);
            agentEffects.AddStatusEffect(appliedLong);

            // Initially, the higher value modifier should be applied
            agentEffects.Tick(agentEffects.TickRate * 1.1f);
            modShort.Received(1).Apply(Arg.Any<float>(), Arg.Any<AgentDrivenProperties>());
            modLong.DidNotReceive().Apply(Arg.Any<float>(), Arg.Any<AgentDrivenProperties>());

            // Advance time to expire the short effect
            agentEffects.Tick(1.1f);

            modLong.Received(1).Apply(Arg.Any<float>(), Arg.Any<AgentDrivenProperties>());
        }
    }
}
