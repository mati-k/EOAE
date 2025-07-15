using EOAE_Code.Data.Managers;
using EOAE_Code.Data.Xml;
using TaleWorlds.CampaignSystem.Settlements;

namespace EOAE_Code.Extensions
{
    public static class SettlementExtensions
    {
        public static SettlementUniqueMilitiaDataXml GetSettlementUniqueMilitia(
            this Settlement settlement
        )
        {
            return SettlementUniqueMilitiaManager.GetSettlementUniqueMilitia(settlement.StringId);
        }
    }
}
