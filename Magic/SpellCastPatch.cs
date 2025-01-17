using System;
using System.Linq;
using EOAE_Code.Character;
using EOAE_Code.Data.Managers;
using EOAE_Code.Magic.Spells;
using HarmonyLib;
using JetBrains.Annotations;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.GauntletUI.Mission;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD;

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
        [HarmonyPatch(
            typeof(ItemObject),
            nameof(WeaponComponentData.RelevantSkill),
            MethodType.Getter
        )]
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
        [HarmonyPatch(
            typeof(WeaponComponentData),
            nameof(WeaponComponentData.RelevantSkill),
            MethodType.Getter
        )]
        public static bool PatchMagicSkillForWeapon(
            WeaponComponentData __instance,
            ref SkillObject __result
        )
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
        public static void ChangeWeaponClassText(
            in EquipmentElement targetWeapon,
            int targetWeaponUsageIndex,
            EquipmentElement comparedWeapon,
            int comparedWeaponUsageIndex,
            bool isInit,
            ItemMenuVM __instance,
            TextObject ____classText
        )
        {
            if (targetWeapon.Item != null && SpellManager.IsSpell(targetWeapon.Item.StringId))
            {
                ItemMenuTooltipPropertyVM property = __instance
                    .TargetItemProperties.Where(property =>
                        property.DefinitionLabel == ____classText.ToString()
                    )
                    .First();
                property.ValueLabel = "Spell";
            }

            if (comparedWeapon.Item != null && SpellManager.IsSpell(comparedWeapon.Item.StringId))
            {
                WeaponComponentData weaponWithUsageIndex =
                    targetWeapon.Item.GetWeaponWithUsageIndex(targetWeaponUsageIndex);
                ItemMenuTooltipPropertyVM property = __instance
                    .ComparedItemProperties.Where(property =>
                        property.ValueLabel
                        == GameTexts
                            .FindText(
                                "str_inventory_weapon",
                                ((int)weaponWithUsageIndex.WeaponClass).ToString()
                            )
                            .ToString()
                    )
                    .First();
                property.ValueLabel = "Spell";
            }
        }

        [HarmonyPatch(typeof(Mission))]
        [HarmonyPatch("OnAgentShootMissile")]
        [UsedImplicitly]
        [MBCallback]
        private class OverrideOnAgentShootMissile
        {
            private static bool Prefix(
                Agent shooterAgent,
                EquipmentIndex weaponIndex,
                bool hasRigidBody
            )
            {
                MissionWeapon missionWeapon = shooterAgent.Equipment[weaponIndex];
                if (SpellManager.IsWeaponSpell(missionWeapon.CurrentUsageItem))
                {
                    if (MagicMissionLogic.CurrentMana.ContainsKey(shooterAgent))
                    {
                        Spell spell = SpellManager.GetSpellFromWeapon(
                            missionWeapon.CurrentUsageItem
                        );

                        if (MagicMissionLogic.CurrentMana[shooterAgent] >= spell.Cost)
                        {
                            MagicMissionLogic.CurrentMana[shooterAgent] -= spell.Cost;

                            if (
                                shooterAgent.IsPlayerControlled
                                || MagicMissionLogic.CurrentMana[shooterAgent] >= spell.Cost
                            )
                            {
                                // Restock for player or AI with enough mana to keep firing without interruption
                                shooterAgent.SetWeaponAmountInSlot(weaponIndex, 1 + 1, true);
                            }

                            if (!spell.IsThrown)
                            {
                                spell.Cast(shooterAgent);

                                return false;
                            }
                        }
                        else if (shooterAgent.IsPlayerControlled)
                        {
                            InformationManager.DisplayMessage(
                                new InformationMessage("Not enough mana.")
                            );
                            shooterAgent.SetWeaponAmountInSlot(weaponIndex, 1 + 1, true);

                            return false;
                        }
                    }
                }

                return true;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Agent), "DropItem")]
        public static bool AgentDropItemPrefix(
            EquipmentIndex itemIndex,
            WeaponClass pickedUpItemType,
            Agent __instance
        )
        {
            // Prevent from spells being droped on ground and costing mana
            if (
                itemIndex == EquipmentIndex.ExtraWeaponSlot
                && SpellManager.IsWeaponSpell(__instance.Equipment[itemIndex].CurrentUsageItem)
            )
            {
                __instance.RemoveEquippedWeapon(itemIndex);
                return false;
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(MissionGauntletCrosshair), "GetShouldCrosshairBeVisible")]
        public static bool PatchGetShouldCrosshairBeVisible(ref bool __result)
        {
            var usingAreaAim = Mission.Current.GetMissionBehavior<SpellAimView>().IsActive;
            if (Mission.Current.MainAgent != null && usingAreaAim)
            {
                __result = false;
                return false;
            }

            return true;
        }
    }
}
