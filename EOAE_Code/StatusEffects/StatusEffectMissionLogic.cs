﻿using System.Collections.Generic;
using EOAE_Code.Data.Managers;
using EOAE_Code.Data.Xml.StatusEffects;
using EOAE_Code.Extensions;
using EOAE_Code.Magic.Spells;
using EOAE_Code.Wrappers;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.StatusEffects
{
    public class StatusEffectMissionLogic : MissionLogic
    {
        // todo: Dictionary or maybe a behavior component on each agent?
        private static readonly Dictionary<Agent, AgentEffects> AgentActiveEffects = new();

        public override void AfterStart()
        {
            base.AfterStart();
            AgentActiveEffects.Clear();
        }

        public override void OnMissionTick(float dt)
        {
            foreach (var agentActiveEffect in AgentActiveEffects)
            {
                agentActiveEffect.Value.Tick(dt);
            }
        }

        public static AgentDrivenProperties? GetAgentStatModifiers(Agent agent)
        {
            AgentActiveEffects.TryGetValue(agent, out var agentStatusEffects);

            if (agentStatusEffects == null)
            {
                return null;
            }

            return agentStatusEffects.AgentPropertiesMultipliers;
        }

        public override void OnAgentHit(
            Agent affectedAgent,
            Agent affectorAgent,
            in MissionWeapon affectorWeapon,
            in Blow blow,
            in AttackCollisionData attackCollisionData
        )
        {
            var statusEffect = GetStatusEffectFromWeapon(affectorWeapon);

            if (statusEffect == null || attackCollisionData.AttackBlockedWithShield)
                return;

            if (!AgentActiveEffects.ContainsKey(affectedAgent))
            {
                AgentActiveEffects.Add(
                    affectedAgent,
                    new AgentEffects(AgentWrapper.GetFor(affectedAgent))
                );
            }

            AgentActiveEffects[affectedAgent]
                .AddStatusEffect(new AppliedStatusEffect(statusEffect, affectorAgent));
        }

        private static StatusEffect? GetStatusEffectFromWeapon(MissionWeapon weapon)
        {
            var missileEffect = weapon.Item.GetMissileEffect();
            if (missileEffect != null)
            {
                return missileEffect;
            }

            if (SpellManager.IsWeaponSpell(weapon.CurrentUsageItem))
            {
                var spell = SpellManager.GetSpellFromWeapon(weapon.CurrentUsageItem);
                if (spell is MissileSpell missileSpell)
                {
                    return missileSpell.Effect;
                }
            }

            if (weapon.Item.IsEnchanted())
            {
                return weapon.Item.GetEnchantment()!.StatusEffect;
            }

            return null;
        }
    }
}
