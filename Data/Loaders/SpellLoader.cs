using EOAE_Code.Data.Xml;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TaleWorlds.Core;
using TaleWorlds.ModuleManager;

namespace EOAE_Code.Data.Loaders
{
    public static class SpellLoader
    {
        private static Dictionary<string, SpellDataXml> spells = new Dictionary<string, SpellDataXml>();
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

        public static SpellDataXml GetSpellFromItem(string itemName)
        {
            return spells[itemName];
        }

        public static SpellDataXml GetSpellFromWeapon(WeaponComponentData weaponComponentData)
        {
            return spells[spellWeapons[weaponComponentData].StringId];
        }

        public static void LoadSpells()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<SpellDataXml>));
            string path = ModuleHelper.GetModuleFullPath("EOAE_Code") + "custom_xml/" + spellFile;
            if (File.Exists(path))
            {
                List<SpellDataXml> loadedSpells = xmlSerializer.Deserialize(File.OpenRead(path)) as List<SpellDataXml> ?? new List<SpellDataXml>();
                foreach (SpellDataXml spell in loadedSpells)
                {
                    spells.Add(spell.ItemName, spell);
                }
            }
        }
    }
}
