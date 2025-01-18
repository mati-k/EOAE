using System;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml
{
    [Serializable]
    public class BombardSpellData
    {
        [XmlAttribute]
        public string MissileName = "";

        [XmlAttribute]
        public int MissileCount = 0;

        [XmlAttribute]
        public float MissileSpeed = 0;

        [XmlAttribute]
        public float MinHeight = 0;

        [XmlAttribute]
        public float MaxHeight = 0;
    }
}
