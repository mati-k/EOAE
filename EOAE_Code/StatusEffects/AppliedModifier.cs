using EOAE_Code.Data.Xml.StatusEffects;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.StatusEffects
{
    public struct AppliedModifier
    {
        public Modifier Modifier { get; private set; }
        public Agent Caster { get; private set; }

        public AppliedModifier(Modifier modifier, Agent caster)
        {
            Modifier = modifier;
            Caster = caster;
        }

        public override bool Equals(object obj)
        {
            if (obj is AppliedModifier other)
            {
                return this.Modifier == other.Modifier && this.Caster == other.Caster;
            }
            return false;
        }

        public static bool operator ==(AppliedModifier left, AppliedModifier right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AppliedModifier left, AppliedModifier right)
        {
            return !(left == right);
        }
    }
}
