using System;
using System.IO;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.ModuleManager;

namespace EOAE_Code.Models
{
    public class CustomVillageTradeModel : VillageTradeModel
    {
        private readonly float tradeBoundDistance;

        public CustomVillageTradeModel()
        {
            tradeBoundDistance = Convert.ToSingle(
                File.ReadAllText(
                    ModuleHelper.GetModuleFullPath("EOAE_Code") + "TradeBoundDistance.txt"
                )
            );
        }

        public override Settlement GetTradeBoundToAssignForVillage(Village village)
        {
            throw new System.NotImplementedException();
        }

        public override float TradeBoundDistanceLimitAsDays(
            MobileParty.NavigationType navigationType
        )
        {
            // Default implementation for now
            return Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(
                    navigationType
                )
                * 3f
                / (
                    Campaign.Current.EstimatedAverageVillagerPartySpeed
                    * (float)CampaignTime.HoursInDay
                );
        }
    }
}
