using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EOAE_Code.SettlementUniqueMilitia
{
    [Serializable]
    public class SettlementUniqueMilitiaModel
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
