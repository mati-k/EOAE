using System;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml.Spells
{
    [Serializable]
    [XmlInclude(typeof(MissileSpellData))]
    [XmlInclude(typeof(HealSelfSpellData))]
    [XmlInclude(typeof(HealGroupSpellData))]
    [XmlInclude(typeof(SummonSpellData))]
    [XmlInclude(typeof(BombardSpellData))]
    [XmlInclude(typeof(AreaSpellData))]
    public class SpellData
    {
        [XmlAttribute]
        public string Name = "";

        [XmlAttribute]
        public string ItemName = "";

        [XmlAttribute("School")]
        public string SchoolName = "";

        [XmlAttribute]
        public int Cost = 0;

        [XmlAttribute]
        public string Icon = "";

        [XmlAttribute]
        public string Animation = "";
    }
}
