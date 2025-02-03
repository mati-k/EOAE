using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Data.Xml.StatusEffects
{
    [Serializable]
    public class MovementSpeedEffectData : StatusEffectBase
    {
        public override void Apply(float totalValue, AgentDrivenProperties multiplierProperties)
        {
            multiplierProperties.MaxSpeedMultiplier = totalValue + 1;
        }

        public override void EffectTick(float totalValue, Agent target, Agent caster) { }
    }
}
