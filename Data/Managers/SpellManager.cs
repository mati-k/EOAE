using EOAE_Code.Data.Xml;
using EOAE_Code.Interfaces;
using EOAE_Code.Magic.Spells;
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
        private static Dictionary<string, Spell> spells = new Dictionary<string, Spell>();
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

        public static Spell GetSpellFromItem(string itemName)
        {
            return spells[itemName];
        }

        public static Spell GetSpellFromWeapon(WeaponComponentData weaponComponentData)
        {
            return spells[spellWeapons[weaponComponentData].StringId];
        }

        public static int GetSpellCount()
        {
            return spells.Count;
        }

        public static Spell GetSpell(int index)
        {
            return spells.Values.ToList()[index];
        }

        public void Add(SpellDataXml item)
        {
            Spell spell;
            switch (item.Effect)
            {
                case "Throw":
                    spell = new MissileSpell(item); break;
                case "HealSelf":
                    spell = new HealSelfSpell(item); break;
                case "HealGroup":
                    spell = new HealGroupSpell(item); break;
                default:
                    spell = new MissileSpell(item); break;
            }

            spells.Add(item.Name, spell);
        }
    }
}
