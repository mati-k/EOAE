using EOAE_Code.Wrappers;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Data.Xml.StatusEffects.Modifiers
{
    public class ResistanceEffectData : Modifier
    {
        private static readonly TextObject SpeedupTextObject = new(
            "{=XjBGc7Bp}Grant {value}% magic resistance"
        );

        public override void Apply(float totalValue, AgentDrivenProperties multiplierProperties)
        {
            // todo: Implement when damage types are added
        }

        public override string GetDescription(float scale)
        {
            return SpeedupTextObject.SetTextVariable("value", Value * scale * 100).ToString();
        }

        public override StatusEffectAction GetScaled(float scale)
        {
            return new ResistanceEffectData { Value = Value * scale };
        }

        public override void Tick(float totalValue, AgentWrapper target, Agent caster) { }
    }
}
