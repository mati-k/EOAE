using System.Collections.Generic;
using EOAE_Code.Data.Xml.Enchantments;
using EOAE_Code.Interfaces;

namespace EOAE_Code.Data.Managers
{
    public class EnchantmentManager : IDataManager<EnchantmentData>
    {
        private static Dictionary<string, EnchantmentData> enchantments = new();

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
    }
}
