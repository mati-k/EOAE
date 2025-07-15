using System;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml.BattleSpellBook
{
    [Serializable]
    public class NobleSpellData
    {
        [XmlAttribute]
        public string Spell = "";

        [XmlAttribute]
        public float Weight = 0;

        [XmlAttribute]
        public string RequiredPerk = "";
    }
}
