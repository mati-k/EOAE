using EOAE_Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml.Spells
{
    [Serializable]
    public class SummonSpellData : SpellData
    {
        [XmlAttribute]
        public float Range = 0;

        [XmlAttribute]
        public float Radius = 0;

        [XmlArray("SummonEntities")]
        [XmlArrayItem("SummonEntity")]
        public List<SummonEntityData> SummonEntities = new();

        [XmlAttribute]
        public string AreaAimPrefab = "";
    }
}
