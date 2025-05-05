using EOAE_Code.Data.Xml.Enchantments;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace EOAE_Code.States.Enchantment
{
    public class EnchantmentEnchantmentVM : EnchantmentDraggable
    {
        [DataSourceProperty]
        public override string Name
        {
            get { return EnchantmentData.DisplayName.ToString(); }
        }

        public EnchantmentData EnchantmentData { get; private set; }

        public bool IsFiltered => throw new System.NotImplementedException();

        public EnchantmentEnchantmentVM() { }

        public EnchantmentEnchantmentVM(bool isInSlot)
        {
            IsInSlot = isInSlot;
            ImageIdentifier = new ImageIdentifierVM();
        }

        public EnchantmentEnchantmentVM(EnchantmentData enchantment)
        {
            this.EnchantmentData = enchantment;
            ImageIdentifier = new ImageIdentifierVM(enchantment.IconItem, ImageIdentifierType.Item);
        }

        public void AssignToSlot(EnchantmentEnchantmentVM enchantment)
        {
            this.EnchantmentData = enchantment.EnchantmentData;
            this.ImageIdentifier = enchantment.ImageIdentifier;
        }

        public override void Clear()
        {
            this.EnchantmentData = null;
            this.ImageIdentifier = new ImageIdentifierVM();
        }
    }
}
