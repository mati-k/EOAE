using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml
{
    [Serializable]
    public class TroopSpellData
    {
        [XmlAttribute]
        public string Spell = "";
        [XmlAttribute]
        public float Weight = 0;
    }
}
