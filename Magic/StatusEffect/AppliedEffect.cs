using EOAE_Code.Data.Xml.StatusEffects;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic.StatusEffect
{
    public class AppliedEffect
    {
        public Effect Effect { get; private set; }
        public Agent Caster { get; private set; }
        public float DurationLeft { get; private set; }

        public AppliedEffect(Effect effect, Agent caster)
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
