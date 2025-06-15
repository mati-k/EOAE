using System;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml.Spells
{
    [Serializable]
    public class BombardSpellData : SpellData
    {
        [XmlAttribute]
        public float Range = 0;

        [XmlAttribute]
        public float Radius = 0;

        [XmlElement("Missile")]
        public BombardMissileData BombardMissile = new();

        [XmlAttribute]
        public string AreaAimPrefab = "";
    }
}
