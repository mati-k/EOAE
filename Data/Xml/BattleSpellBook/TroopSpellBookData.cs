using System;
using System.Linq;
using System.Xml.Serialization;
using EOAE_Code.Data.Managers;
using EOAE_Code.Interfaces;
using EOAE_Code.Magic.Spells;
using TaleWorlds.Core;

namespace EOAE_Code.Data.Xml.BattleSpellBook
{
    [Serializable]
    public class TroopSpellBookData : IBattleSpellBook
    {
        [XmlAttribute]
        public string TroopId = "";

        [XmlArray]
        public TroopSpellData[] Spells = new TroopSpellData[0];

        public void Normalize()
        {
            var totalWeight = Spells.Sum(it => it.Weight);

            for (int i = 0; i < Spells.Length; i++)
            {
                Spells[i].Weight /= totalWeight;
            }
        }

        public Spell GetRandomSpell()
        {
            float random = MBRandom.RandomFloat;
            float currentWeight = 0;
            for (int i = 0; i < Spells.Length; i++)
            {
                currentWeight += Spells[i].Weight;
                if (random <= currentWeight)
                {
                    return SpellManager.GetSpellFromItem(Spells[i].Spell);
                }
            }

            throw new Exception("No spell found");
        }
    }
}
