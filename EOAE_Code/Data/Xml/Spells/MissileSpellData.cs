using System;
using System.Xml.Serialization;
using EOAE_Code.Data.Xml.StatusEffects;

namespace EOAE_Code.Data.Xml.Spells
{
    [Serializable]
    public class MissileSpellData : SpellData
    {
        [XmlAttribute]
        public int ExtraMissiles = 0;

        [XmlAttribute]
        public float MissileSpread = 1;

        [XmlElement]
        public StatusEffect? Effect;
    }
}
