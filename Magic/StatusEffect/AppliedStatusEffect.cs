using EOAE_Code.Data.Xml.StatusEffects;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic.StatusEffect
{
    public class AppliedStatusEffect
    {
        public StatusEffectBase StatusEffect { get; private set; }
        public Agent Caster { get; private set; }
        public float DurationLeft { get; private set;}

        public AppliedStatusEffect(StatusEffectBase statusEffect, Agent caster)
        {
            StatusEffect = statusEffect;
            Caster = caster;
            DurationLeft = statusEffect.Duration;
        }

        public void Tick(float dt)
        {
            DurationLeft -= dt;
        }
    }
}
