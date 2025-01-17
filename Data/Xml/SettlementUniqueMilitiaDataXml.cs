using System;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml
{
    [Serializable]
    public class SettlementUniqueMilitiaDataXml
    {
        [XmlAttribute]
        public string SettlementId = "";

        [XmlAttribute]
        public string MeleeMilitiaId = "";

        [XmlAttribute]
        public string MeleeEliteMilitiaId = "";

        [XmlAttribute]
        public string RangedMilitiaId = "";

        [XmlAttribute]
        public string RangedEliteMilitiaId = "";
    }
}
