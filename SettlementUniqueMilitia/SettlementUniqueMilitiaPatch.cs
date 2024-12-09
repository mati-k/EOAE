using EOAE_Code.Data.Xml;
using EOAE_Code.Extensions;
using HarmonyLib;
using SandBox.CampaignBehaviors;
using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.ObjectSystem;

namespace EOAE_Code.SettlementUniqueMilitia
{
    [HarmonyPatch]
    public class SettlementUniqueMilitiaPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Settlement), "AddMilitiasToParty")]
        public static bool AddMilitiasToPartyOverride(MobileParty militaParty, int militiaToAdd, Settlement __instance)
        {
            SettlementUniqueMilitiaDataXml settlementUniqueMilitia = __instance.GetSettlementUniqueMilitia();
            // Use culture militia if settlement got no custom defined
            if (settlementUniqueMilitia == null)
            {
                return true;
            }


            float troopRatio;
            float num;
            Campaign.Current.Models.SettlementMilitiaModel.CalculateMilitiaSpawnRate(__instance, out troopRatio, out num);

            var method = AccessTools.Method(typeof(Settlement), "AddTroopToMilitiaParty");

            object[] parameters = new object[]
            {
                militaParty, MBObjectManager.Instance.GetObject<CharacterObject>(settlementUniqueMilitia.MeleeMilitiaId), MBObjectManager.Instance.GetObject<CharacterObject>(settlementUniqueMilitia.MeleeEliteMilitiaId), troopRatio, militiaToAdd
            };
            method.Invoke(__instance, parameters);

            method.Invoke(__instance, new object[]
            {
                militaParty, MBObjectManager.Instance.GetObject<CharacterObject>(settlementUniqueMilitia.RangedMilitiaId), MBObjectManager.Instance.GetObject<CharacterObject>(settlementUniqueMilitia.RangedEliteMilitiaId), 1f, parameters[parameters.Length - 1]
            });


            // Remove initial culture militia units
            if (militaParty.MemberRoster.FindIndexOfTroop(__instance.Culture.MeleeMilitiaTroop) != -1)
            {
                militaParty.MemberRoster.RemoveTroop(__instance.Culture.MeleeMilitiaTroop);
                militaParty.MemberRoster.RemoveTroop(__instance.Culture.RangedMilitiaTroop);
            }

            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(GuardsCampaignBehavior), "InitializeGarrisonCharacters")]
        public static bool InitializeGarrisonCharactersOverride(Settlement settlement, List<ValueTuple<CharacterObject, int>> ____garrisonTroops)
        {
            ____garrisonTroops.Clear();
            if (Campaign.Current.GameMode == CampaignGameMode.Campaign)
            {
                MobileParty militiaParty = settlement.MilitiaPartyComponent.MobileParty;
                if (militiaParty == null)
                {
                    return true;
                }

                foreach (TroopRosterElement troopRosterElement in militiaParty.MemberRoster.GetTroopRoster())
                {
                    if (troopRosterElement.Character.Occupation == Occupation.Soldier)
                    {
                        ____garrisonTroops.Add(new ValueTuple<CharacterObject, int>(troopRosterElement.Character, troopRosterElement.Number - troopRosterElement.WoundedNumber));
                    }
                }
            }

            return false;
        }
    }
}
