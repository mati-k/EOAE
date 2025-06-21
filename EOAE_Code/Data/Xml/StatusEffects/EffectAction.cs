using System;
using System.Xml.Serialization;
using EOAE_Code.Wrappers;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Data.Xml.StatusEffects
{
    [XmlInclude(typeof(MovementSpeedEffectData))]
    [XmlInclude(typeof(DamageOverTimeEffectData))]
    [Serializable]
    public abstract class EffectAction
    {
        [XmlAttribute]
        public float Value = 0;

        public abstract void Apply(float totalValue, AgentDrivenProperties multiplierProperties);

        public abstract void Tick(float totalValue, AgentWrapper target, Agent caster);
    }
}
