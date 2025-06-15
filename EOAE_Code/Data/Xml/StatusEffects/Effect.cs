using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Data.Xml.StatusEffects
{
    [Serializable]
    public class Effect
    {
        public List<EffectAction> Actions = new List<EffectAction>();

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
                action.Tick(totalValue, target, caster);
            }
        }
    }
}
