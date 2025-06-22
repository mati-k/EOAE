using TaleWorlds.MountAndBlade;

namespace EOAE_Code.StatusEffects
{
    public class AppliedStatusEffect
    {
        public Data.Xml.StatusEffects.StatusEffect Effect { get; private set; }
        public Agent Caster { get; private set; }
        public float DurationLeft { get; private set; }

        public AppliedStatusEffect(Data.Xml.StatusEffects.StatusEffect effect, Agent caster)
        {
            Effect = effect;
            Caster = caster;
            DurationLeft = effect.Duration;
        }

        public void Tick(float dt)
        {
            DurationLeft -= dt;
        }
    }
}
