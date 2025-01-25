using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using EOAE_Code.Data.Xml;
using EOAE_Code.Interfaces;
using TaleWorlds.ModuleManager;

namespace EOAE_Code.Data.Loaders
{
    public static class XmlDataLoader
    {
        private static XmlStorageClass LoadXmlData<XmlElementClass, XmlStorageClass>(string file)
            where XmlElementClass : class
            where XmlStorageClass : class, new()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(XmlStorageClass));
            string path = ModuleHelper.GetModuleFullPath("EOAE_Code") + "custom_xml/" + file;
            if (File.Exists(path))
            {
                XmlStorageClass loadedData =
                    xmlSerializer.Deserialize(File.OpenRead(path)) as XmlStorageClass
                    ?? new XmlStorageClass();

                return loadedData;
            }

            return new XmlStorageClass();
        }

        public static void LoadXmlDataList<XmlElementClass, DataManagerClass>(string file)
            where XmlElementClass : class
            where DataManagerClass : IDataManager<XmlElementClass>, new()
        {
            DataManagerClass dataManager = new DataManagerClass();
            var loadedData = LoadXmlData<XmlElementClass, List<XmlElementClass>>(file);
           
            foreach (XmlElementClass dataEntry in loadedData)
            {
                dataManager.Add(dataEntry);
            }
        }

        // Could not make the Storage class directly IEnuerable, caused class to be directly read as array and ignored the XmlArrayItem annotations
        public static void LoadXmlDataCustomRoot<XmlElementClass, DataManagerClass, XmlStorageClass>(string file)
            where XmlElementClass : class
            where DataManagerClass : IDataManager<XmlElementClass>, new()
            where XmlStorageClass : class, IGetDataList<XmlElementClass>, new()
        {
            DataManagerClass dataManager = new DataManagerClass();
            var loadedData = LoadXmlData<XmlElementClass, XmlStorageClass>(file).GetDataList();

            foreach (XmlElementClass dataEntry in loadedData)
            {
                dataManager.Add(dataEntry);
            }
        }
    }
}
