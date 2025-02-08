using EOAE_Code.Data.Xml.StatusEffects;
using System;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml.Spells
{
    [Serializable]
    public class MissileSpellData : SpellData
    {
        [XmlElement]
        public StatusEffectBase? StatusEffect;
    }
}
