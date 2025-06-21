using System;
using EOAE_Code.Wrappers;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Data.Xml.StatusEffects
{
    [Serializable]
    public class MovementSpeedEffectData : Modifier
    {
        public override void Apply(float totalValue, AgentDrivenProperties multiplierProperties)
        {
            multiplierProperties.MaxSpeedMultiplier = totalValue + 1;
        }

        public override void Tick(float totalValue, AgentWrapper target, Agent caster) { }
    }
}
