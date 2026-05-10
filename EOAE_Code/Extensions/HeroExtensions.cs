using System.Collections.Generic;
using System.Linq;
using EOAE_Code.Data.Xml.BattleSpellBook;
using EOAE_Code.Magic;
using EOAE_Code.Magic.Spells;
using TaleWorlds.CampaignSystem;

namespace EOAE_Code.Extensions
{
    public static class HeroExtensions
    {
        public static List<Spell> GetPickedSpellSlots(this Hero hero)
        {
            var heroSpellBook = Campaign
                .Current.GetCampaignBehavior<SpellBookCampaignBehavior>()
                .HeroSpellBooks;
            return heroSpellBook.TryGetValue(hero, out var spells) ? spells : new List<Spell>();
        }

        public static List<Spell> GetPickedSpells(this Hero hero)
        {
            return GetPickedSpells(hero).Where(spell => spell != null).ToList();
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
            var spellBookCampaignBehavior =
                Campaign.Current.GetCampaignBehavior<SpellBookCampaignBehavior>();
            spellBookCampaignBehavior.SaveHeroSpellBook(hero, spells);
        }
    }
}
