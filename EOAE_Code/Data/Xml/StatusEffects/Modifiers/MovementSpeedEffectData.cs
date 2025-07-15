using System;
using EOAE_Code.Wrappers;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Data.Xml.StatusEffects
{
    [Serializable]
    public class MovementSpeedEffectData : Modifier
    {
        private static readonly TextObject SpeedupTextObject = new(
            "{=lhJ1SPoM}Slow down by {value}%"
        );
        private static readonly TextObject SlowdownTextObject = new(
            "{=pSm32bJu}Increase speed by {value}%"
        );

        public override void Apply(float totalValue, AgentDrivenProperties multiplierProperties)
        {
            multiplierProperties.MaxSpeedMultiplier = totalValue + 1;
        }

        public override void Tick(float totalValue, AgentWrapper target, Agent caster) { }

        public override StatusEffectAction GetScaled(float scale)
        {
            return new MovementSpeedEffectData { Value = Value * scale, Key = Key };
        }

        public override string GetDescription(float scale)
        {
            float percentageValue = Value * scale * 100;

            if (Value > 0)
            {
                return SpeedupTextObject.SetTextVariable("value", percentageValue).ToString();
            }
            else if (Value < 0)
            {
                return SlowdownTextObject.SetTextVariable("value", percentageValue).ToString();
            }

            return string.Empty;
        }
    }
}
