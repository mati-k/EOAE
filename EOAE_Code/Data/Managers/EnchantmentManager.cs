using System.Collections.Generic;
using EOAE_Code.Data.Xml.Enchantments;
using EOAE_Code.Interfaces;

namespace EOAE_Code.Data.Managers
{
    public class EnchantmentManager : IDataManager<EnchantmentData>
    {
        private static Dictionary<string, EnchantmentData> enchantments = new();
        private static Dictionary<string, float> soulGemValues = new()
        {
            { "iron", 1 },
            { "ironIngot1", 1.5f },
            { "ironIngot2", 2f },
            { "ironIngot3", 2.5f },
            { "ironIngot4", 3f },
            { "ironIngot5", 3.5f },
        };

        public void Add(EnchantmentData item)
        {
            if (!enchantments.ContainsKey(item.Name))
            {
                enchantments.Add(item.Name, item);
            }
        }

        public static List<EnchantmentData> GetAllEnchantments()
        {
            return new List<EnchantmentData>(enchantments.Values);
        }

        public static float GetSoulGemValue(string itemName)
        {
            if (soulGemValues.TryGetValue(itemName, out float value))
            {
                return value;
            }

            return 1f;
        }

        public static EnchantmentData? GetEnchantment(string name)
        {
            if (enchantments.TryGetValue(name, out EnchantmentData? enchantment))
            {
                return enchantment;
            }

            return null;
        }
    }
}
