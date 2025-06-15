using System.Collections.Generic;
using System.Linq;
using EOAE_Code.Data.Xml.BattleSpellBook;
using EOAE_Code.Magic.Spells;
using TaleWorlds.CampaignSystem;

namespace EOAE_Code.Extensions
{
    public static class HeroExtensions
    {
        // ToDo: saveing this
        private static Dictionary<Hero, List<Spell>> heroPickedSpells = new();

        public static List<Spell> GetPickedSpellSlots(this Hero hero)
        {
            return heroPickedSpells.TryGetValue(hero, out var spells) ? spells : new List<Spell>();
        }

        public static List<Spell> GetPickedSpells(this Hero hero)
        {
            return heroPickedSpells.TryGetValue(hero, out var spells)
                ? spells.Where(spell => spell != null).ToList()
                : new List<Spell>();
        }

        public static CompanionSpellBook? GetCompanionSpellBook(this Hero hero)
        {
            var spells = GetPickedSpells(hero);
            if (spells.Count > 0)
            {
                return new CompanionSpellBook(spells);
            }

            return null;
        }

        public static void SetPickedSpells(this Hero hero, List<Spell> spells)
        {
            heroPickedSpells[hero] = spells;
        }
    }
}
