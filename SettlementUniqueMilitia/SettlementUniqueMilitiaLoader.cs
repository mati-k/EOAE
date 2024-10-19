using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TaleWorlds.ModuleManager;

namespace EOAE_Code.SettlementUniqueMilitia
{
    public static class SettlementUniqueMilitiaLoader
    {
        private static Dictionary<string, SettlementUniqueMilitiaModel> settlementsMilitia = new Dictionary<string, SettlementUniqueMilitiaModel>();
        private static string militiaFile = "custom_militia.xml";

        public static SettlementUniqueMilitiaModel GetSettlementUniqueMilitia(string settlementId)
        {
            SettlementUniqueMilitiaModel? militia;
            SettlementUniqueMilitiaLoader.settlementsMilitia.TryGetValue(settlementId, out militia);
            return militia;
        }

        public static void LoadSettlements()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<SettlementUniqueMilitiaModel>));
            string path = ModuleHelper.GetModuleFullPath("EOAE_Code") + "custom_xml/" + militiaFile;
            if (File.Exists(path))
            {
                List<SettlementUniqueMilitiaModel> settlementMilitias = xmlSerializer.Deserialize(File.OpenRead(path)) as List<SettlementUniqueMilitiaModel> ?? new List<SettlementUniqueMilitiaModel>();
                foreach (SettlementUniqueMilitiaModel settlementMilitia in settlementMilitias)
                {
                    SettlementUniqueMilitiaLoader.settlementsMilitia.Add(settlementMilitia.SettlementId, settlementMilitia);
                }
            }
        }
    }
}
