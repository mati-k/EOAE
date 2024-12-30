using EOAE_Code.Data.Xml;
using EOAE_Code.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;

namespace EOAE_Code.Data.Managers
{
    public class SpellManager : IDataManager<SpellDataXml>
    {
        private static Dictionary<string, SpellDataXml> spells = new Dictionary<string, SpellDataXml>();
        private static Dictionary<WeaponComponentData, ItemObject> spellWeapons = new Dictionary<WeaponComponentData, ItemObject>();

        public static bool IsSpell(string itemName)
        {
            return spells.ContainsKey(itemName);
        }

        public static void AddWeaponSpell(ItemObject itemObject, WeaponComponentData weaponComponentData)
        {
            if (spells.ContainsKey(itemObject.StringId))
            {
                spellWeapons.Add(weaponComponentData, itemObject);
            }
        }

        public static bool IsWeaponSpell(WeaponComponentData weaponComponentData)
        {
            return spellWeapons.ContainsKey(weaponComponentData);
        }

        public static SpellDataXml GetSpellFromItem(string itemName)
        {
            return spells[itemName];
        }

        public static SpellDataXml GetSpellFromWeapon(WeaponComponentData weaponComponentData)
        {
            return spells[spellWeapons[weaponComponentData].StringId];
        }

        public static int GetSpellCount()
        {
            return spells.Count;
        }

        public static SpellDataXml GetSpell(int index)
        {
            return spells.Values.ToList()[index];
        }

        public void Add(SpellDataXml item)
        {
            spells.Add(item.Name, item);
        }
    }
}
