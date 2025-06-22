using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using EOAE_Code.Wrappers;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Data.Xml.StatusEffects
{
    [Serializable]
    public class StatusEffect
    {
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
    }
}
