using System;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml.StatusEffects
{
    [Serializable]
    public class StatusEffectTemplate
    {
        [XmlElement("Effect")]
        public StatusEffect Effect = new StatusEffect();

        public StatusEffect GetScaled(float scale)
        {
            return Effect.GetScaled(scale);
        }

        public string GetDescription(float value)
        {
            return Effect.GetDescription(value);
        }
    }
}
