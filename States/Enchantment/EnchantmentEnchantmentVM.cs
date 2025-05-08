using EOAE_Code.Data.Xml.Enchantments;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace EOAE_Code.States.Enchantment
{
    public class EnchantmentEnchantmentVM : EnchantmentDraggable
    {
        private bool _isFiltered;

        [DataSourceProperty]
        public override string Name
        {
            get { return EnchantmentData.DisplayName.ToString(); }
        }

        public EnchantmentData EnchantmentData { get; private set; }

        [DataSourceProperty]
        public bool IsFiltered
        {
            get { return _isFiltered; }
            set
            {
                if (value != this._isFiltered)
                {
                    this._isFiltered = value;
                    base.OnPropertyChangedWithValue(value, "IsFiltered");
                }
            }
        }

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

        public void FilterToItem(ItemRosterElement? item)
        {
            if (item == null || EnchantmentData == null)
            {
                IsFiltered = false;
                return;
            }

            IsFiltered = !EnchantmentData.ItemTypes.Contains(item.Value.EquipmentElement.Item.Type);
        }
    }
}
