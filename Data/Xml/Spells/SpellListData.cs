using EOAE_Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace EOAE_Code.Data.Xml.Spells
{
    [Serializable]
    [XmlRoot("SpellList")]
    public class SpellListData : IGetDataList<SpellData>
    {
        [XmlArrayItem("Missile", typeof(MissileSpellData))]
        [XmlArrayItem("HealSelf", typeof(HealSelfSpellData))]
        [XmlArrayItem("HealGroup", typeof(HealGroupSpellData))]
        [XmlArrayItem("Summon", typeof(SummonSpellData))]
        [XmlArrayItem("Bombard", typeof(BombardSpellData))]
        public List<SpellData> Spells = new List<SpellData>();

        public List<SpellData> GetDataList()
        {
            return Spells;
        }
    }
}
