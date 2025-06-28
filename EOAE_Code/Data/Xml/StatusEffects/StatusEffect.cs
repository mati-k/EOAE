using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using EOAE_Code.Extensions;
using EOAE_Code.Wrappers;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Data.Xml.StatusEffects
{
    [Serializable]
    public class StatusEffect
    {
        [XmlArrayItem("DamageOverTime", typeof(DamageOverTimeEffectData))]
        [XmlArrayItem("MovementSpeed", typeof(MovementSpeedEffectData))]
        public List<StatusEffectAction> Actions = new List<StatusEffectAction>();

        [XmlAttribute]
        public float Duration = 0;

        public void Apply(float totalValue, AgentDrivenProperties multiplierProperties)
        {
            foreach (var action in Actions)
            {
                action.Apply(totalValue, multiplierProperties);
            }
        }

        public void EffectTick(float totalValue, Agent target, Agent caster)
        {
            foreach (var action in Actions)
            {
                action.Tick(totalValue, AgentWrapper.GetFor(target), caster);
            }
        }

        public StatusEffect GetScaled(float scale)
        {
            var scaledEffect = new StatusEffect { Duration = Duration };
            foreach (var action in Actions)
            {
                scaledEffect.Actions.Add(action.GetScaled(scale));
            }
            return scaledEffect;
        }

        public string GetDescription(float value)
        {
            var descriptionBuilder = new System.Text.StringBuilder();
            foreach (var action in Actions)
            {
                descriptionBuilder.AppendLine(action.GetDescription(value));
            }
            return descriptionBuilder.ToString().TrimEnd();
        }

        public void AddTooltips(ItemMenuVM itemMenuVM)
        {
            foreach (var action in Actions)
            {
                itemMenuVM.AddTooltip("", action.GetDescription(), Color.Black);
            }
        }

        public void AddTooltips(List<TooltipProperty> tooltips)
        {
            foreach (var action in Actions)
            {
                tooltips.Add(new TooltipProperty("", action.GetDescription(), 0));
            }
        }
    }
}
