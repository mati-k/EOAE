using System.Collections.Generic;
using EOAE_Code.Magic.Spells;
using TaleWorlds.CampaignSystem;

namespace EOAE_Code.Extensions
{
    public static class HeroExtensions
    {
        // ToDo: saveing this
        private static Dictionary<Hero, List<Spell>> heroPickedSpells = new();

        public static List<Spell>? GetPickedSpells(this Hero hero)
        {
            return heroPickedSpells.TryGetValue(hero, out var spells) ? spells : null;
        }

        public static void SetPickedSpells(this Hero hero, List<Spell> spells)
        {
            heroPickedSpells[hero] = spells;
        }
    }
}
