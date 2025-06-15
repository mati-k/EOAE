using System.Collections.Generic;
using EOAE_Code.Interfaces;
using EOAE_Code.Magic.Spells;
using TaleWorlds.Core;

namespace EOAE_Code.Data.Xml.BattleSpellBook
{
    public class CompanionSpellBook : IBattleSpellBook
    {
        private List<Spell> spells;

        public CompanionSpellBook(List<Spell> spells)
        {
            this.spells = spells;
        }

        public Spell GetRandomSpell()
        {
            return spells.GetRandomElement();
        }
    }
}
