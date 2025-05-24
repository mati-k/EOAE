using System;
using System.Collections.Generic;
using System.Linq;
using EOAE_Code.Data.Managers;
using EOAE_Code.Interfaces;
using EOAE_Code.Magic.Spells;
using TaleWorlds.Core;

namespace EOAE_Code.Data.Xml.BattleSpellBook
{
    public class NobleBattleSpells : IBattleSpellBook
    {
        private List<NobleSpellData> spells;
        private float totalWeight;

        public NobleBattleSpells(List<NobleSpellData> spells)
        {
            this.spells = spells;
            this.totalWeight = spells.Sum(spell => spell.Weight);
        }

        public Spell GetRandomSpell()
        {
            float random = MBRandom.RandomFloat * totalWeight;
            float currentWeight = 0;
            for (int i = 0; i < spells.Count; i++)
            {
                currentWeight += spells[i].Weight;
                if (random <= currentWeight)
                {
                    return SpellManager.GetSpellFromItem(spells[i].Spell);
                }
            }

            throw new Exception("No spell found");
        }
    }
}
