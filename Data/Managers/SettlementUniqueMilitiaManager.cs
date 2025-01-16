using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EOAE_Code.Data.Xml;
using EOAE_Code.Interfaces;

namespace EOAE_Code.Data.Managers
{
    public class SettlementUniqueMilitiaManager : IDataManager<SettlementUniqueMilitiaDataXml>
    {
        private static Dictionary<string, SettlementUniqueMilitiaDataXml> settlementsMilitia =
            new();

        public static SettlementUniqueMilitiaDataXml GetSettlementUniqueMilitia(string settlementId)
        {
            SettlementUniqueMilitiaDataXml? militia;
            settlementsMilitia.TryGetValue(settlementId, out militia);
            return militia;
        }

        void IDataManager<SettlementUniqueMilitiaDataXml>.Add(SettlementUniqueMilitiaDataXml item)
        {
            settlementsMilitia.Add(item.SettlementId, item);
        }
    }
}
