using System;
using System.Xml.Serialization;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Data.Xml.StatusEffects
{
    [XmlInclude(typeof(MovementSpeedEffectData))]
    [XmlInclude(typeof(DamageOverTimeEffectData))]
    [Serializable]
    public abstract class StatusEffectBase
    {
        [XmlAttribute]
        // Key like freezing , burning, to avoid stacking from same source
        public string Key = "";

        [XmlAttribute]
        public float Value = 0;

        [XmlAttribute]
        public float Duration = 0;

        public abstract void Apply(float totalValue, AgentDrivenProperties multiplierProperties);

        public abstract void EffectTick(float totalValue, Agent target, Agent caster);
    }
}
