using System;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml
{
    [Serializable]
    public class SpellDataXml
    {
        [XmlAttribute]
        public string Name = "";
        [XmlAttribute]
        public string ItemName = "";
        [XmlAttribute]
        public string School = "";
        [XmlAttribute]
        public int Cost = 0;
    }
}
