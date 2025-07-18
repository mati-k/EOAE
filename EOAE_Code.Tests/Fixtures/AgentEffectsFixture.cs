﻿using EOAE_Code.StatusEffects;
using EOAE_Code.Wrappers;
using TaleWorlds.Engine;

namespace EOAE_Code.Tests.Fixtures
{
    public class AgentEffectsFixture : AgentEffects
    {
        public AgentEffectsFixture(AgentWrapper agent)
            : base(agent) { }

        public List<AppliedStatusEffect> ActiveEffects => activeEffects;
        public Dictionary<
            string,
            TaleWorlds.Library.PriorityQueue<float, AppliedModifier>
        > ExclusiveModifiers => exclusiveModifiers;
        public List<AppliedModifier> StackableModifiers => stackableModifiers;
        public Dictionary<string, GameEntity> ParticleEffects => particleEffects;

        public float TickRate => EFFECT_TICK_RATE;
    }
}
