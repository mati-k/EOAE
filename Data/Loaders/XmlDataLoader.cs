using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using EOAE_Code.Interfaces;
using TaleWorlds.ModuleManager;

namespace EOAE_Code.Data.Loaders
{
    public static class XmlDataLoader
    {
        public static void LoadXmlData<XmlDataClass, DataManagerClass>(string file)
            where XmlDataClass : class
            where DataManagerClass : IDataManager<XmlDataClass>, new()
        {
            DataManagerClass dataManager = new DataManagerClass();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<XmlDataClass>));
            string path = ModuleHelper.GetModuleFullPath("EOAE_Code") + "custom_xml/" + file;
            if (File.Exists(path))
            {
                List<XmlDataClass> loadedData =
                    xmlSerializer.Deserialize(File.OpenRead(path)) as List<XmlDataClass>
                    ?? new List<XmlDataClass>();
                foreach (XmlDataClass dataEntry in loadedData)
                {
                    dataManager.Add(dataEntry);
                }
            }
        }
    }
}
