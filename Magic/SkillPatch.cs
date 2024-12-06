using HarmonyLib;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace EOAE_Code.Magic
{
    [HarmonyPatch]
    public static class SkillPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Game), "InitializeDefaultGameObjects")]
        public static void LoadSkills()
        {
            Attributes.Instance.Initialize();
            Skills.Instance.Initialize();
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(WeaponComponentData))]
        [HarmonyPatch(MethodType.Constructor)]
        [HarmonyPatch(new Type[] { typeof(ItemObject), typeof(WeaponClass), typeof(WeaponFlags) })]
        public static void StoreSpellWeapon(ref ItemObject item, WeaponComponentData __instance)
        {
            SpellLoader.AddWeaponSpell(item, __instance);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(WeaponComponentData), nameof(WeaponComponentData.RelevantSkill), MethodType.Getter)]
        public static bool PatchMagicSkillForWeapon(WeaponComponentData __instance, ref SkillObject __result)
        {
            if (SpellLoader.IsWeaponSpell(__instance))
            {
                __result = Skills.Instance.Destruction;
                return false;
            }

            return true;
        }

        [HarmonyPatch(typeof(Mission))]
        [HarmonyPatch("OnAgentShootMissile")]
        [UsedImplicitly]
        [MBCallback]
        private class OverrideOnAgentShootMissile
        {
            private static bool Prefix(Agent shooterAgent, EquipmentIndex weaponIndex)
            {
                MissionWeapon missionWeapon = shooterAgent.Equipment[weaponIndex];
                if (SpellLoader.IsWeaponSpell(missionWeapon.CurrentUsageItem))
                {
                    shooterAgent.SetWeaponAmountInSlot(weaponIndex, 1+1, true);
                 
                    if (MagicMissionLogic.CurrentMana.ContainsKey(shooterAgent))
                    {
                        int spellCost = SpellLoader.GetSpellFromWeapon(missionWeapon.CurrentUsageItem).Cost;

                        if (MagicMissionLogic.CurrentMana[shooterAgent] >= spellCost)
                        {
                            MagicMissionLogic.CurrentMana[shooterAgent] -= spellCost;
                        }
                        else
                        {
                            InformationManager.DisplayMessage(new InformationMessage("Not enough mana."));
                            return false;
                        }
                    }

                }

                return true;
            }
        }
    }
}
