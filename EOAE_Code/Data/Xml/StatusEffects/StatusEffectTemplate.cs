using System;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml.StatusEffects
{
    [Serializable]
    public class StatusEffectTemplate
    {
        [XmlElement("Effect")]
        public StatusEffect Effect = new StatusEffect();
        public float Scale = 1f;
    }
}
