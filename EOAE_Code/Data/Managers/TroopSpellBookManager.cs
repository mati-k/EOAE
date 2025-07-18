﻿using System;
using System.Collections.Generic;
using EOAE_Code.Data.Xml.BattleSpellBook;
using EOAE_Code.Interfaces;

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

        public static TroopSpellBookData? GetSpellBookForTroop(string troopId)
        {
            if (troopSpellBooks.ContainsKey(troopId))
            {
                return troopSpellBooks[troopId];
            }

            return null;
        }
    }
}
