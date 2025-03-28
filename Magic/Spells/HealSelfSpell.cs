using System;
using EOAE_Code.Character;
using EOAE_Code.Data.Xml.Spells;
using EOAE_Code.Extensions;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic.Spells
{
    public class HealSelfSpell : Spell
    {
        private const float UNRESTRICTED_HEAL_MANA = 70;
        private const float MIN_HEALED_SPELL_PERCENTAGE = 0.6f;

        public override bool IsThrown => false;

        public float HealValue { get; private set; }

        public HealSelfSpell(SpellData data)
            : base(data)
        {
            HealSelfSpellData healSelfSpellData = data as HealSelfSpellData;
            HealValue = healSelfSpellData.HealValue;
        }

        public override void Cast(Agent caster)
        {
            var healValueWithBonus =
                HealValue
                * caster.GetMultiplierForSkill(
                    CustomSkills.Instance.Restoration,
                    CustomSkillEffects.Instance.RestorationHeal
                );

            float originalHealth = caster.Health;
            caster.Health = Math.Min(caster.Health + healValueWithBonus, caster.HealthLimit);
            caster.AddSkillXp(
                CustomSkills.Instance.Restoration,
                MagicConstants.RESTORATION_EXP_PER_HEALTHPOINT * (caster.Health - originalHealth)
            );
        }

        // Cast only if either has enough surplus mana or the heal is effective enough
        public override bool IsAICastValid(Agent caster)
        {
            float currentMana = MagicMissionLogic.AgentsMana[caster].CurrentMana;

            if (caster.Health >= caster.HealthLimit)
            {
                return false;
            }

            bool isAboveMinimumEffectiveHeal =
                caster.HealthLimit - caster.Health >= HealValue * MIN_HEALED_SPELL_PERCENTAGE;
            if (currentMana < UNRESTRICTED_HEAL_MANA && !isAboveMinimumEffectiveHeal)
            {
                return false;
            }

            return true;
        }
    }
}
