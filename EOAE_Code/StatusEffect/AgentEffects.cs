using System;
using System.Collections.Generic;
using System.Linq;
using EOAE_Code.Data.Managers;
using EOAE_Code.Data.Xml.StatusEffects;
using EOAE_Code.Wrappers;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.StatusEffect
{
    public class AgentEffects
    {
        protected const float EFFECT_TICK_RATE = 1;
        private float tickCounter = EFFECT_TICK_RATE;

        public AgentWrapper Agent { get; private set; }
        protected List<AppliedStatusEffect> activeEffects = new();
        protected Dictionary<string, PriorityQueue<float, Modifier>> exclusiveModifiers = new();
        protected List<Modifier> stackableModifiers = new();

        protected Dictionary<string, GameEntity> particleEffects = new();

        public AgentDrivenProperties AgentPropertiesMultipliers { get; private set; } = new();

        public AgentEffects(AgentWrapper agent)
        {
            Agent = agent;
        }

        public void AddStatusEffect(AppliedStatusEffect appliedStatusEffect)
        {
            // ToDo: Add Tests after adding instant stuff
            if (appliedStatusEffect.Effect.Actions.Any(action => action is Modifier))
            {
                activeEffects.Add(appliedStatusEffect);
            }

            foreach (var action in appliedStatusEffect.Effect.Actions)
            {
                if (action is not Modifier modifier)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(modifier.Key))
                {
                    stackableModifiers.Add(modifier);
                }
                else
                {
                    if (!exclusiveModifiers.ContainsKey(modifier.Key))
                    {
                        exclusiveModifiers.Add(modifier.Key, new());
                    }

                    exclusiveModifiers[modifier.Key].Enqueue(Math.Abs(modifier.Value), modifier);

                    string key = modifier.Key;
                    if (StatusEffectParticleManager.StatusEffectPrefabs.ContainsKey(key))
                    {
                        string particlePrefab = StatusEffectParticleManager.StatusEffectPrefabs[
                            key
                        ];
                        if (!particleEffects.ContainsKey(particlePrefab))
                        {
                            AddParticleEffect(particlePrefab);
                        }
                    }
                }
            }
        }

        public void Tick(float dt)
        {
            if (!Agent.IsActive())
            {
                return;
            }

            TickAndCleanupAppliedEffects(dt);

            foreach (var exclusiveModifier in exclusiveModifiers)
            {
                string key = exclusiveModifier.Key;

                if (exclusiveModifier.Value.IsEmpty && particleEffects.ContainsKey(key))
                {
                    particleEffects[key].FadeOut(0, true);
                    particleEffects.Remove(key);
                }
            }

            // Todo: maybe do this every 1s or so to save performance
            RecalculateStats();
            Agent.UpdateAgentProperties();
            FireEffectTick(dt);
            UpdateEffectPositions();
        }

        private void RecalculateStats()
        {
            AgentPropertiesMultipliers.MaxSpeedMultiplier = 1;

            foreach (var exclusiveModifier in exclusiveModifiers)
            {
                if (!exclusiveModifier.Value.IsEmpty)
                {
                    var modifier = exclusiveModifier.Value.PeekValue();
                    modifier.Apply(modifier.Value, AgentPropertiesMultipliers);
                }
            }

            foreach (var modifier in stackableModifiers)
            {
                modifier.Apply(modifier.Value, AgentPropertiesMultipliers);
            }
        }

        private void FireEffectTick(float dt)
        {
            tickCounter -= dt;

            if (tickCounter < 0)
            {
                foreach (var exclusiveModifier in exclusiveModifiers)
                {
                    if (!exclusiveModifier.Value.IsEmpty)
                    {
                        var modifier = exclusiveModifier.Value.PeekValue();
                        modifier.Tick(modifier.Value, Agent, null);
                    }
                }

                foreach (var modifier in stackableModifiers)
                {
                    modifier.Tick(modifier.Value, Agent, null);
                }

                tickCounter = EFFECT_TICK_RATE;
            }
        }

        private void AddParticleEffect(string prefab)
        {
            var effectEntity = GameEntity.CreateEmpty(Mission.Current.Scene);
            MatrixFrame frame = MatrixFrame.Identity;
            var particle = ParticleSystem.CreateParticleSystemAttachedToEntity(
                prefab,
                effectEntity,
                ref frame
            );
            var globalFrame = new MatrixFrame(Mat3.Identity, Agent.Position);
            effectEntity.SetGlobalFrame(globalFrame);

            particleEffects.Add(prefab, effectEntity);
        }

        private void UpdateEffectPositions()
        {
            var globalFrame = new MatrixFrame(Mat3.Identity, Agent.Position);
            foreach (var particleEffect in particleEffects)
            {
                particleEffect.Value.SetGlobalFrame(globalFrame);
            }
        }

        private void TickAndCleanupAppliedEffects(float dt)
        {
            for (int i = activeEffects.Count - 1; i >= 0; i--)
            {
                activeEffects[i].Tick(dt);
                if (activeEffects[i].DurationLeft <= 0)
                {
                    CleanUpAppliedEffect(activeEffects[i]);
                    activeEffects.RemoveAt(i);
                }
            }
        }

        private void CleanUpAppliedEffect(AppliedStatusEffect appliedEffect)
        {
            foreach (var action in appliedEffect.Effect.Actions)
            {
                if (action is Modifier modifier)
                {
                    if (string.IsNullOrEmpty(modifier.Key))
                    {
                        stackableModifiers.Remove(modifier);
                    }
                    else
                    {
                        if (exclusiveModifiers.ContainsKey(modifier.Key))
                        {
                            exclusiveModifiers[modifier.Key]
                                .Remove(
                                    new KeyValuePair<float, Modifier>(
                                        Math.Abs(modifier.Value),
                                        modifier
                                    )
                                );
                        }
                    }
                }
            }
        }
    }
}
