using System;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml.Spells
{
    [Serializable]
    public class HealGroupSpellData : SpellData
    {
        [XmlAttribute]
        public float HealValue = 0;
        [XmlAttribute]
        public float HealRange = 0;
    }
}
