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
            var scaledEffect = new StatusEffect { Duration = Effect.Duration };
            foreach (var action in Effect.Actions)
            {
                scaledEffect.Actions.Add(action.GetScaled(scale));
            }

            return scaledEffect;
        }
    }
}
