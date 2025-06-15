using System;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml.BattleSpellBook
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
