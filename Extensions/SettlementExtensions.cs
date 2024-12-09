using EOAE_Code.Data.Loaders;
using EOAE_Code.Data.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Engine;

namespace EOAE_Code.Extensions
{
    public static class SettlementExtensions
    {
        public static SettlementUniqueMilitiaDataXml GetSettlementUniqueMilitia(this Settlement settlement)
        {
            return SettlementUniqueMilitiaLoader.GetSettlementUniqueMilitia(settlement.StringId);
        }

    }
}
