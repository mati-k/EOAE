using System;
using System.Xml.Serialization;
using EOAE_Code.Data.Xml.StatusEffects;

namespace EOAE_Code.Data.Xml
{
    [Serializable]
    public class BombardMissileData
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

        [XmlElement]
        public Effect? Effect;
    }
}
