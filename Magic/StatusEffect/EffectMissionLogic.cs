using System.Collections.Generic;
using EOAE_Code.Data.Managers;
using EOAE_Code.Magic.Spells;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic.StatusEffect
{
    public class EffectMissionLogic : MissionLogic
    {
        // todo: Dictionary or maybe a behavior component on each agent?
        private static Dictionary<Agent, AgentEffects> AgentActiveEffects = new();

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
            if (!SpellManager.IsWeaponSpell(affectorWeapon.CurrentUsageItem))
            {
                return;
            }

            var spell = SpellManager.GetSpellFromWeapon(affectorWeapon.CurrentUsageItem);
            if (spell is MissileSpell missileSpell && missileSpell.Effect != null)
            {
                if (!AgentActiveEffects.ContainsKey(affectedAgent))
                {
                    AgentActiveEffects.Add(affectedAgent, new AgentEffects(affectedAgent));
                }

                AgentActiveEffects[affectedAgent]
                    .AddStatusEffect(new AppliedEffect(missileSpell.Effect, affectorAgent));
            }
        }
    }
}
