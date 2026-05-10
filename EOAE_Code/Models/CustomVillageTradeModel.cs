using System;
using System.IO;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.ModuleManager;

namespace EOAE_Code.Models
{
    public class CustomVillageTradeModel : DefaultVillageTradeModel
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

        public override float TradeBoundDistanceLimitAsDays(
            MobileParty.NavigationType navigationType
        )
        {
            return tradeBoundDistance;
        }
    }
}
