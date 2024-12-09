using EOAE_Code.Data.Xml;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TaleWorlds.ModuleManager;

namespace EOAE_Code.Data.Loaders
{
    public static class SettlementUniqueMilitiaLoader
    {
        private static Dictionary<string, SettlementUniqueMilitiaDataXml> settlementsMilitia = new Dictionary<string, SettlementUniqueMilitiaDataXml>();
        private static string militiaFile = "custom_militia.xml";

        public static SettlementUniqueMilitiaDataXml GetSettlementUniqueMilitia(string settlementId)
        {
            SettlementUniqueMilitiaDataXml? militia;
            settlementsMilitia.TryGetValue(settlementId, out militia);
            return militia;
        }

        public static void LoadSettlements()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<SettlementUniqueMilitiaDataXml>));
            string path = ModuleHelper.GetModuleFullPath("EOAE_Code") + "custom_xml/" + militiaFile;
            if (File.Exists(path))
            {
                List<SettlementUniqueMilitiaDataXml> settlementMilitias = xmlSerializer.Deserialize(File.OpenRead(path)) as List<SettlementUniqueMilitiaDataXml> ?? new List<SettlementUniqueMilitiaDataXml>();
                foreach (SettlementUniqueMilitiaDataXml settlementMilitia in settlementMilitias)
                {
                    settlementsMilitia.Add(settlementMilitia.SettlementId, settlementMilitia);
                }
            }
        }
    }
}
