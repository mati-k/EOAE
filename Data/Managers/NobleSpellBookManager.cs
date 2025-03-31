using System;
using System.Collections.Generic;
using EOAE_Code.Data.Xml.BattleSpellBook;
using EOAE_Code.Interfaces;
using TaleWorlds.CampaignSystem;

namespace EOAE_Code.Data.Managers
{
    public class NobleSpellBookManager : IDataManager<NobleSpellBookData>
    {
        private static Dictionary<string, NobleSpellBookData> nobleSpellBooks = new();

        public void Add(NobleSpellBookData item)
        {
            if (!nobleSpellBooks.ContainsKey(item.Culture))
            {
                nobleSpellBooks.Add(item.Culture, item);
            }
            else
            {
                throw new Exception($"NobleSpellbook with Culture {item.Culture} already exists.");
            }
        }

        public static NobleSpellBookData? GetSpellBookForCulture(CultureObject culture)
        {
            {
                if (nobleSpellBooks.ContainsKey(culture.StringId))
                {
                    return nobleSpellBooks[culture.StringId];
                }

                return null;
            }
        }
    }
}
