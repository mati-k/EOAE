using System;
using System.Collections.Generic;
using System.Linq;
using EOAE_Code.Data.Managers;
using EOAE_Code.Extensions;
using EOAE_Code.Magic.StatusEffect;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic
{
    public class AgentStatusEffects
    {
        private const float EFFECT_TICK_RATE = 1;
        private float tickCounter = EFFECT_TICK_RATE;

        public Agent Agent { get; private set; }
        private Dictionary<Type, List<AppliedStatusEffect>> ActiveStatusEffects = new();
        private Dictionary<string, GameEntity> ParticleEffects = new();

        public AgentDrivenProperties AgentPropertiesMultipliers { get; private set; } = new();

        public AgentStatusEffects(Agent agent)
        {
            Agent = agent;
        }

        public void AddStatusEffect(AppliedStatusEffect appliedStatusEffect)
        {
            if (!ActiveStatusEffects.ContainsKey(appliedStatusEffect.StatusEffect.GetType()))
            {
                ActiveStatusEffects.Add(
                    appliedStatusEffect.StatusEffect.GetType(),
                    new List<AppliedStatusEffect>()
                );
            }

            ActiveStatusEffects[appliedStatusEffect.StatusEffect.GetType()]
                .Add(appliedStatusEffect);

            string key = appliedStatusEffect.StatusEffect.Key;
            if (StatusEffectParticleManager.StatusEffectPrefabs.ContainsKey(key))
            {
                string particlePrefab = StatusEffectParticleManager.StatusEffectPrefabs[key];
                if (!ParticleEffects.ContainsKey(particlePrefab))
                {
                    AddParticleEffect(particlePrefab);
                }
            }
        }

        public void Tick(float dt)
        {
            if (!Agent.IsActive())
            {
                return;
            }

            foreach (var statusEffectsList in ActiveStatusEffects.Values)
            {
                HashSet<string> removedEffects = new();
                for (int i = statusEffectsList.Count - 1; i >= 0; i--)
                {
                    statusEffectsList[i].Tick(dt);
                    if (statusEffectsList[i].DurationLeft <= 0)
                    {
                        removedEffects.Add(statusEffectsList[i].StatusEffect.Key);
                        statusEffectsList.RemoveAt(i);
                    }
                }

                foreach (var effectKey in removedEffects)
                {
                    if (!StatusEffectParticleManager.StatusEffectPrefabs.ContainsKey(effectKey))
                    {
                        continue;
                    }

                    string particlePrefab = StatusEffectParticleManager.StatusEffectPrefabs[
                        effectKey
                    ];
                    if (
                        ParticleEffects.ContainsKey(particlePrefab)
                        && !statusEffectsList.Any(effect => effect.StatusEffect.Key == effectKey)
                    )
                    {
                        ParticleEffects[particlePrefab].FadeOut(0, true);
                        ParticleEffects.Remove(particlePrefab);
                    }
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

            foreach (var statusEffectList in ActiveStatusEffects.Values)
            {
                if (statusEffectList.Count == 0)
                {
                    continue;
                }

                // Find maximum values of each type and sum them to get final effect
                // i.e. freeze type slowing and potion type increasing speed, finds max of each and gets final
                var summedEffectValue = statusEffectList
                    .GroupBy(effect => effect.StatusEffect.Key)
                    .Select(group =>
                        group.MaxByCustom(effect => Math.Abs(effect.StatusEffect.Value))
                    )
                    .Select(effect => effect.StatusEffect.Value)
                    .Sum();

                statusEffectList
                    .First()
                    .StatusEffect.Apply(summedEffectValue, AgentPropertiesMultipliers);
            }
        }

        private void FireEffectTick(float dt)
        {
            tickCounter -= dt;

            if (tickCounter < 0)
            {
                foreach (var statusEffectList in ActiveStatusEffects.Values)
                {
                    if (statusEffectList.Count == 0)
                    {
                        continue;
                    }

                    // Find maximum values of each type and sum them to get final effect
                    // i.e. freeze type slowing and potion type increasing speed, finds max of each and gets final
                    var summedEffectValue = statusEffectList
                        .GroupBy(effect => effect.StatusEffect.Key)
                        .Select(group =>
                            group.MaxByCustom(effect => Math.Abs(effect.StatusEffect.Value))
                        )
                        .Select(effect => effect.StatusEffect.Value)
                        .Sum();

                    statusEffectList
                        .First()
                        .StatusEffect.EffectTick(
                            summedEffectValue,
                            Agent,
                            statusEffectList.First().Caster
                        );
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

            ParticleEffects.Add(prefab, effectEntity);
        }

        private void UpdateEffectPositions()
        {
            var globalFrame = new MatrixFrame(Mat3.Identity, Agent.Position);
            foreach (var particleEffect in ParticleEffects)
            {
                particleEffect.Value.SetGlobalFrame(globalFrame);
            }
        }
    }
}
