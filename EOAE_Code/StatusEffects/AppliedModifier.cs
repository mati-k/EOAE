using System;
using EOAE_Code.Data.Xml.StatusEffects;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.StatusEffects
{
    public struct AppliedModifier : IEquatable<AppliedModifier>
    {
        public Modifier Modifier { get; private set; }
        public Agent Caster { get; private set; }

        public AppliedModifier(Modifier modifier, Agent caster)
        {
            Modifier = modifier;
            Caster = caster;
        }

        public bool Equals(AppliedModifier other)
        {
            return Modifier == other.Modifier && Caster == other.Caster;
        }
    }
}
