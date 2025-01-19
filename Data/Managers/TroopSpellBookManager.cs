using EOAE_Code.Data.Xml;
using EOAE_Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EOAE_Code.Data.Managers
{
    public class TroopSpellBookManager : IDataManager<TroopSpellBookData>
    {
        private static Dictionary<string, TroopSpellBookData> troopSpellBooks = new();

        public void Add(TroopSpellBookData item)
        {
            if (!troopSpellBooks.ContainsKey(item.TroopId))
            {
                item.Normalize();
                troopSpellBooks.Add(item.TroopId, item);
            }
            else
            {
                throw new Exception($"TroopSpellBook with TroopId {item.TroopId} already exists.");
            }
        }

        public static TroopSpellBookData? GetSpellBooxForTroop(string troopId)
        {
            if (troopSpellBooks.ContainsKey(troopId))
            {
                return troopSpellBooks[troopId];
            }

            return null;
        }
    }
}
