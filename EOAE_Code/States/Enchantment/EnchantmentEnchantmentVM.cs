using EOAE_Code.Data.Xml.Enchantments;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace EOAE_Code.States.Enchantment
{
    public class EnchantmentEnchantmentVM : EnchantmentDraggable
    {
        private EnchantmentData _enchantmentData;
        private bool _isFiltered;

        [DataSourceProperty]
        public override string Name
        {
            get { return new TextObject(EnchantmentData.DisplayName).ToString(); }
        }

        public EnchantmentData EnchantmentData
        {
            get { return _enchantmentData; }
            private set
            {
                this._enchantmentData = value;
                OnItemChanged();
            }
        }

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
            this.ImageIdentifier = enchantment.ImageIdentifier;
            this.EnchantmentData = enchantment.EnchantmentData;
        }

        public override void Clear()
        {
            this.ImageIdentifier = new ImageIdentifierVM();
            this.EnchantmentData = null;
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
