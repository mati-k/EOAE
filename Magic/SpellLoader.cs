using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TaleWorlds.Core;
using TaleWorlds.ModuleManager;

namespace EOAE_Code.Magic
{
    public static class SpellLoader
    {
        private static Dictionary<string, SpellModel> spells = new Dictionary<string, SpellModel>();
        private static Dictionary<WeaponComponentData, ItemObject> spellWeapons = new Dictionary<WeaponComponentData, ItemObject>();

        private static string spellFile = "spells.xml";

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

        public static SpellModel GetSpellFromWeapon(WeaponComponentData weaponComponentData)
        {
            return spells[spellWeapons[weaponComponentData].StringId];
        }

        public static void LoadSpells()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<SpellModel>));
            string path = ModuleHelper.GetModuleFullPath("EOAE_Code") + "custom_xml/" + spellFile;
            if (File.Exists(path))
            {
                List<SpellModel> loadedSpells = xmlSerializer.Deserialize(File.OpenRead(path)) as List<SpellModel> ?? new List<SpellModel>();
                foreach (SpellModel spell in loadedSpells)
                {
                    SpellLoader.spells.Add(spell.ItemName, spell);
                }
            }
        }
    }
}
