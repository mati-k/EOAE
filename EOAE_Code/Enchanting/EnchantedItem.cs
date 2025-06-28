using EOAE_Code.Data.Managers;
using EOAE_Code.Data.Xml.Enchantments;
using EOAE_Code.Data.Xml.StatusEffects;
using TaleWorlds.SaveSystem;

namespace EOAE_Code.Enchanting
{
    [SaveableRootClass(3)]
    public class EnchantedItem
    {
        [SaveableField(1)]
        public string EnchantmentName;

        [SaveableField(2)]
        public float Scale;

        public EnchantmentData Enchantment;

        public StatusEffect StatusEffect;

        public EnchantedItem(EnchantmentData enchantment, float scale)
        {
            Enchantment = enchantment;
            EnchantmentName = enchantment.Name;
            Scale = scale;
        }

        public EnchantedItem(string name, float scale)
        {
            EnchantmentName = name;
            Scale = scale;
        }

        public void LoadEnchantment()
        {
            if (Enchantment == null)
            {
                Enchantment = EnchantmentManager.GetEnchantment(EnchantmentName)!;
            }

            StatusEffect = Enchantment.StatusEffectTemplate.GetScaled(Scale);
        }
    }
}
