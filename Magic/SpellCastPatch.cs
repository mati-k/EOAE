using EOAE_Code.Character;
using EOAE_Code.Data.Loaders;
using EOAE_Code.Data.Managers;
using EOAE_Code.Data.Xml;
using EOAE_Code.Magic.Spells;
using HarmonyLib;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD;
using TaleWorlds.ObjectSystem;

namespace EOAE_Code.Magic
{
    [HarmonyPatch]
    public static class SpellCastPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(WeaponComponentData))]
        [HarmonyPatch(MethodType.Constructor)]
        [HarmonyPatch(new Type[] { typeof(ItemObject), typeof(WeaponClass), typeof(WeaponFlags) })]
        public static void StoreSpellWeapon(ref ItemObject item, WeaponComponentData __instance)
        {
            SpellManager.AddWeaponSpell(item, __instance);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ItemObject), nameof(WeaponComponentData.RelevantSkill), MethodType.Getter)]
        public static bool PatchMagicSkillForWeapon(ItemObject __instance, ref SkillObject __result)
        {
            if (SpellManager.IsSpell(__instance.StringId))
            {
                __result = SpellManager.GetSpellFromItem(__instance.StringId).School;
                return false;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(WeaponComponentData), nameof(WeaponComponentData.RelevantSkill), MethodType.Getter)]
        public static bool PatchMagicSkillForWeapon(WeaponComponentData __instance, ref SkillObject __result)
        {
            if (SpellManager.IsWeaponSpell(__instance))
            {
                __result = CustomSkills.Instance.Destruction;
                return false;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MissionMainAgentEquipmentControllerVM), "GetItemTypeAsString")]
        public static bool GetItemTypeAsStringPrefix(ItemObject item, ref string __result)
        {
            if (SpellManager.IsSpell(item.StringId))
            {
                __result = "Spell";
                return false;
            }

            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(ItemMenuVM), "SetWeaponComponentTooltip")]
        public static void ChangeWeaponClassText(in EquipmentElement targetWeapon, int targetWeaponUsageIndex, EquipmentElement comparedWeapon, int comparedWeaponUsageIndex, bool isInit, ItemMenuVM __instance, TextObject ____classText)
        {
            if (targetWeapon.Item != null && SpellManager.IsSpell(targetWeapon.Item.StringId))
            {
                ItemMenuTooltipPropertyVM property = __instance.TargetItemProperties.Where(property => property.DefinitionLabel == ____classText.ToString()).First();
                property.ValueLabel = "Spell";
            }

            if (comparedWeapon.Item != null && SpellManager.IsSpell(comparedWeapon.Item.StringId))
            {
                WeaponComponentData weaponWithUsageIndex = targetWeapon.Item.GetWeaponWithUsageIndex(targetWeaponUsageIndex);
                ItemMenuTooltipPropertyVM property = __instance.ComparedItemProperties.Where(property => property.ValueLabel == GameTexts.FindText("str_inventory_weapon", ((int)weaponWithUsageIndex.WeaponClass).ToString()).ToString()).First();
                property.ValueLabel = "Spell";
            }
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
                if (SpellManager.IsWeaponSpell(missionWeapon.CurrentUsageItem))
                {
                    shooterAgent.SetWeaponAmountInSlot(weaponIndex, 1+1, true);
                 
                    if (MagicMissionLogic.CurrentMana.ContainsKey(shooterAgent))
                    {
                        Spell spell = SpellManager.GetSpellFromWeapon(missionWeapon.CurrentUsageItem);

                        if (MagicMissionLogic.CurrentMana[shooterAgent] >= spell.Cost)
                        {
                            MagicMissionLogic.CurrentMana[shooterAgent] -= spell.Cost;

                            if (!spell.IsThrown)
                            {
                                spell.Cast(shooterAgent);
                                
                                return false;
                            }
                        }

                        else
                        {
                            if (shooterAgent.IsPlayerControlled)
                            {
                                InformationManager.DisplayMessage(new InformationMessage("Not enough mana."));
                            }
                            else
                            {
                                shooterAgent.SetFiringOrder(FiringOrder.RangedWeaponUsageOrderEnum.HoldYourFire);
                            }

                            return false;
                        }
                    }

                }

                return true;
            }
        }
    }
}
