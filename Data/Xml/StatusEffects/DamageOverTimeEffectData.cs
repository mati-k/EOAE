using System;
using EOAE_Code.Extensions;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Data.Xml.StatusEffects
{
    [Serializable]
    public class DamageOverTimeEffectData : StatusEffectBase
    {
        public override void Apply(float totalValue, AgentDrivenProperties multiplierProperties) { }

        public override void EffectTick(float totalValue, Agent target, Agent caster)
        {
            if (target.IsFadingOut())
            {
                return;
            }

            target.DealDamage(caster, totalValue);
        }
    }
}
