using System;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml.Enchantments
{
    [Serializable]
    public class EnchantmentData
    {
        [XmlAttribute]
        public string Name = "";

        [XmlAttribute]
        public string DisplayName = "";

        // For now use preview from some items, in future maybe custom pictures
        [XmlAttribute]
        public string IconItem = "";
    }
}
