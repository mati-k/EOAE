using System;
using EOAE_Code.Agents;
using EOAE_Code.Character;
using EOAE_Code.Data.Managers;
using EOAE_Code.Extensions;
using EOAE_Code.Interfaces;
using EOAE_Code.Magic.Spells;
using HarmonyLib;
using JetBrains.Annotations;
using SandBox.GameComponents;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.GauntletUI.Mission;

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
                __result = SpellManager.GetSpellFromWeapon(__instance).School;
                return false;
            }

            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(SandboxAgentStatCalculateModel))]
        [HarmonyPatch("GetWeaponDamageMultiplier")]
        public static void GetWeaponDamageMultiplier(
            Agent agent,
            WeaponComponentData weapon,
            ref float __result
        )
        {
            // If more logic of model needs to be changed could inherit and override instead
            var skill = weapon?.RelevantSkill;

            if (skill == CustomSkills.Instance.Destruction && SpellManager.IsWeaponSpell(weapon!))
            {
                __result = agent.GetMultiplierForSkill(
                    CustomSkills.Instance.Destruction,
                    CustomSkillEffects.Instance.DestructionDamage
                );
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
                    if (MagicMissionLogic.AgentsMana.ContainsKey(shooterAgent))
                    {
                        var agentMana = MagicMissionLogic.AgentsMana[shooterAgent];
                        Spell spell = SpellManager.GetSpellFromWeapon(
                            missionWeapon.CurrentUsageItem
                        );

                        if (agentMana.CurrentMana >= spell.Cost)
                        {
                            agentMana.Consume(spell.Cost);

                            if (
                                shooterAgent.IsPlayerControlled
                                || agentMana.CurrentMana >= spell.Cost
                            )
                            {
                                // Restock for player or AI with enough mana to keep firing without interruption
                                shooterAgent.SetWeaponAmountInSlot(weaponIndex, 1 + 1, true);
                            }

                            if (!spell.IsThrown)
                            {
                                try
                                {
                                    spell.Cast(shooterAgent);
                                }
                                catch (Exception e)
                                {
                                    InformationManager.DisplayMessage(
                                        new InformationMessage(
                                            $"ERROR: {shooterAgent.Name} failed to cast {spell.Name} spell: {e.Message}",
                                            Color.FromUint(0xFF0000)
                                        )
                                    );
                                    agentMana.Consume(-spell.Cost);
                                }

                                return false;
                            }
                        }
                        else if (shooterAgent.IsPlayerControlled)
                        {
                            InformationManager.DisplayMessage(
                                new InformationMessage(
                                    new TextObject("{=mbNhD53v}Not enough mana.").ToString()
                                )
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
            var spell = Agent.Main?.GetEquippedSpell();
            if (spell != null && (spell is IUseAreaAim || !spell.IsThrown))
            {
                __result = false;
                return false;
            }

            return true;
        }
    }
}
