using EOAE_Code.Agents;
using HarmonyLib;
using SandBox.Missions.MissionLogics;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic;

[HarmonyPatch]
public static class SummonCombatPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(BattleAgentLogic), "EnemyHitReward")]
    public static bool PatchEnemyHitReward(
        Agent affectedAgent,
        Agent affectorAgent,
        float lastSpeedBonus,
        float lastShotDifficulty,
        bool isSiegeEngineHit,
        WeaponComponentData lastAttackerWeapon,
        AgentAttackType attackType,
        float hitpointRatio,
        float damageAmount
    )
    {
        var summonedAgentComponent = affectorAgent.GetComponent<SummonedAgentComponent>();
        if (summonedAgentComponent != null)
        {
            // No reward on summoned agent hit
            // Maybe for later - reward caster with some experience when summoned agent hits
            return false;
        }

        return true;
    }
}
