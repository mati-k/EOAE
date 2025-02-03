using System.Collections.Generic;
using System.Linq;
using EOAE_Code.Data.Xml.Spells;
using EOAE_Code.Interfaces;
using EOAE_Code.Magic.Spells;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.Core;

namespace EOAE_Code.Data.Managers
{
    public class SpellManager : IDataManager<SpellData>
    {
        private static Dictionary<string, Spell> spells = new();
        private static Dictionary<WeaponComponentData, ItemObject> spellWeapons = new();

        public static bool IsSpell(string itemName)
        {
            return spells.ContainsKey(itemName);
        }

        public static void AddWeaponSpell(
            ItemObject itemObject,
            WeaponComponentData weaponComponentData
        )
        {
            if (spells.ContainsKey(itemObject.StringId))
            {
                spellWeapons.Add(weaponComponentData, itemObject);
            }
        }

        public static bool IsWeaponSpell(WeaponComponentData weaponComponentData)
        {
            if (weaponComponentData == null)
            {
                return false;
            }

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

        public static List<Spell> GetAllSpell()
        {
            return spells.Values.ToList();
        }

        public void Add(SpellData item)
        {
            if (item is MissileSpellData)
            {
                spells.Add(item.ItemName, new MissileSpell(item));
            }
            else if (item is HealSelfSpellData)
            {
                spells.Add(item.ItemName, new HealSelfSpell(item));
            }
            else if (item is HealGroupSpellData)
            {
                spells.Add(item.ItemName, new HealGroupSpell(item));
            }
            else if (item is SummonSpellData)
            {
                spells.Add(item.ItemName, new SummonSpell(item));
            }
            else if (item is BombardSpellData)
            {
                spells.Add(item.ItemName, new BombardSpell(item));
            }
        }

        public static void Clear()
        {
            spells.Clear();
        }
    }
}
