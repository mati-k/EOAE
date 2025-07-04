﻿using System;
using EOAE_Code.Character;
using EOAE_Code.Data.Xml.Spells;
using EOAE_Code.Extensions;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic.Spells
{
    public class HealGroupSpell : Spell
    {
        private const float UNRESTRICTED_HEAL_MANA = 90;
        private const float MIN_HEALED_SPELL_PERCENTAGE = 3f;

        public override bool IsThrown => false;

        public float HealValue { get; private set; }
        public float HealRange { get; private set; }

        public HealGroupSpell(SpellData data)
            : base(data)
        {
            HealGroupSpellData healGroupSpellData = data as HealGroupSpellData;
            HealValue = healGroupSpellData.HealValue;
            HealRange = healGroupSpellData.HealRange;
        }

        public override void Cast(Agent caster)
        {
            var agents = new MBList<Agent>(Mission.Current.Agents);
            Mission.Current.GetNearbyAllyAgents(
                caster.Position.AsVec2,
                HealRange,
                caster.Team,
                agents
            );

            var healValueWithBonus =
                HealValue
                * caster.GetMultiplierForSkill(
                    CustomSkills.Instance.Restoration,
                    CustomSkillEffects.Instance.RestorationHeal
                );

            float healedValue = 0;
            foreach (var agent in agents)
            {
                float originalHealth = agent.Health;
                agent.Health = Math.Min(agent.Health + healValueWithBonus, agent.HealthLimit);
                healedValue += agent.Health - originalHealth;
            }
            caster.AddSkillXp(
                CustomSkills.Instance.Restoration,
                healedValue * MagicConstants.RESTORATION_EXP_PER_HEALTHPOINT
            );
        }

        // Cast only if either has enough surplus mana or the heal is effective enough over surrounding troops
        public override bool IsAICastValid(Agent caster)
        {
            float currentMana = MagicMissionLogic.AgentsMana[caster].CurrentMana;

            var agents = new MBList<Agent>(Mission.Current.Agents);
            Mission.Current.GetNearbyAllyAgents(
                caster.Position.AsVec2,
                HealRange,
                caster.Team,
                agents
            );

            float healedAmount = 0;
            foreach (var agent in agents)
            {
                healedAmount += Math.Min(agent.HealthLimit - agent.Health, HealValue);
            }

            if (healedAmount == 0)
            {
                return false;
            }

            bool isAboveMinimumEffectiveHeal =
                healedAmount >= HealValue * MIN_HEALED_SPELL_PERCENTAGE;
            if (currentMana < UNRESTRICTED_HEAL_MANA && !isAboveMinimumEffectiveHeal)
            {
                return false;
            }

            return true;
        }
    }
}
