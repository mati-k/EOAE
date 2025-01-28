using EOAE_Code.Data.Xml.StatusEffects;
using EOAE_Code.Extensions;
using EOAE_Code.Magic.StatusEffect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic
{
    public class AgentStatusEffects
    {
        public Agent Agent { get; private set; }
        private Dictionary<Type, List<AppliedStatusEffect>> ActiveStatusEffects = new();

        public AgentDrivenProperties AgentPropertiesMultipliers { get; private set; } = new();

        public AgentStatusEffects(Agent agent)
        {
            Agent = agent;
        }

        public void AddStatusEffect(AppliedStatusEffect appliedStatusEffect)
        {
            if (!ActiveStatusEffects.ContainsKey(appliedStatusEffect.StatusEffect.GetType()))
            {
                ActiveStatusEffects.Add(appliedStatusEffect.StatusEffect.GetType(), new List<AppliedStatusEffect>());
            }

            ActiveStatusEffects[appliedStatusEffect.StatusEffect.GetType()].Add(appliedStatusEffect);

            // todo: Temporary test thing, statuses will have optional field for partice effects?
            if (appliedStatusEffect.StatusEffect is MovementSpeedEffectData)
            {
                Agent.AgentVisuals.SetContourColor(Colors.Blue.ToUnsignedInteger(), true);
                Agent.AgentDrivenProperties.MaxSpeedMultiplier = 1 + appliedStatusEffect.StatusEffect.Value;
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
                for (int i = statusEffectsList.Count - 1; i >= 0; i--)
                {
                    statusEffectsList[i].Tick(dt);
                    if (statusEffectsList[i].DurationLeft <= 0)
                    {
                        statusEffectsList.RemoveAt(i);
                    }
                }

                // Todo: checking for end of individual key (like freeze) and removing particle effects
                if (statusEffectsList.Count == 0)
                {
                    Agent.AgentVisuals.SetContourColor(null, true);
                }
            }

            // Todo: maybe do this every 1s or so to save performance
            RecalculateStats();
            Agent.UpdateAgentProperties();
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
                    .Select(group => group.MaxByCustom(effect => Math.Abs(effect.StatusEffect.Value)))
                    .Select(effect => effect.StatusEffect.Value)
                    .Sum();

                statusEffectList.First().StatusEffect.Apply(summedEffectValue, AgentPropertiesMultipliers);
            }
        }
    }
}
