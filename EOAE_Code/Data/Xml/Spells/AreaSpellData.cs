using System;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml.Spells
{
    [Serializable]
    public class AreaSpellData : SpellData
    {
        [XmlAttribute]
        public float Range = 0;

        [XmlAttribute]
        public float Radius = 0;

        [XmlElement("Effect")]
        public AreaEffectData Effect = new();

        [XmlAttribute]
        public string AreaAimPrefab = "";
    }
}
