using System;
using System.Collections.Generic;
using EOAE_Code.Data.Managers;
using EOAE_Code.Data.Xml.StatusEffects;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic.StatusEffect
{
    public class AgentEffects
    {
        private const float EFFECT_TICK_RATE = 1;
        private float tickCounter = EFFECT_TICK_RATE;

        public Agent Agent { get; private set; }
        protected List<AppliedEffect> activeEffects = new();
        protected Dictionary<
            string,
            TaleWorlds.Library.PriorityQueue<float, Modifier>
        > exclusiveModifiers = new();
        protected List<Modifier> stackableModifiers = new List<Modifier>();

        protected Dictionary<string, GameEntity> particleEffects = new();

        public AgentDrivenProperties AgentPropertiesMultipliers { get; private set; } = new();

        public AgentEffects(Agent agent)
        {
            Agent = agent;
        }

        public void AddStatusEffect(AppliedEffect appliedStatusEffect)
        {
            foreach (var action in appliedStatusEffect.Effect.Actions)
            {
                var modifier = action as Modifier;
                if (modifier == null)
                {
                    continue;
                }

                if (String.IsNullOrEmpty(modifier.Key))
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

            CleanUpAppliedEffects();

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

        private void CleanUpAppliedEffects()
        {
            for (int i = activeEffects.Count - 1; i >= 0; i--)
            {
                if (activeEffects[i].DurationLeft <= 0)
                {
                    CleanUpAppliedEffect(activeEffects[i]);
                    activeEffects.RemoveAt(i);
                }
            }
        }

        private void CleanUpAppliedEffect(AppliedEffect appliedEffect)
        {
            foreach (var action in appliedEffect.Effect.Actions)
            {
                if (action is Modifier modifier)
                {
                    if (String.IsNullOrEmpty(modifier.Key))
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
