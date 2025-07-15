using System;
using EOAE_Code.Wrappers;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Data.Xml.StatusEffects
{
    [Serializable]
    public class DamageOverTimeEffectData : Modifier
    {
        private static readonly TextObject DamageTextObject = new(
            "{=WLl0rTcB}Deal {value} damage over time"
        );

        public override void Apply(float totalValue, AgentDrivenProperties multiplierProperties) { }

        public override void Tick(float totalValue, AgentWrapper target, Agent caster)
        {
            if (target.IsFadingOut())
            {
                return;
            }

            target.DealDamage(caster, totalValue);
        }

        public override StatusEffectAction GetScaled(float scale)
        {
            return new DamageOverTimeEffectData { Value = Value * scale, Key = Key };
        }

        public override string GetDescription(float scale)
        {
            return DamageTextObject.SetTextVariable("value", Value * scale).ToString();
        }
    }
}
