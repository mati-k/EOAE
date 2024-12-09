using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
