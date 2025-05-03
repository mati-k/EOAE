using TaleWorlds.Core;
using TaleWorlds.Library;

namespace EOAE_Code.States.Enchantment
{
    public class EnchantmentItemVM : ViewModel
    {
        private ImageIdentifierVM _imageIdentifier;
        private int _itemCount;

        [DataSourceProperty]
        public ImageIdentifierVM ImageIdentifier
        {
            get { return this._imageIdentifier; }
            set
            {
                if (value != this._imageIdentifier)
                {
                    this._imageIdentifier = value;
                    base.OnPropertyChangedWithValue<ImageIdentifierVM>(value, "ImageIdentifier");
                }
            }
        }

        [DataSourceProperty]
        public string Name
        {
            get { return item.EquipmentElement.GetModifiedItemName().ToString(); }
        }

        [DataSourceProperty]
        public int ItemCount
        {
            get { return _itemCount; }
            set
            {
                if (value != this._itemCount)
                {
                    this._itemCount = value;
                    base.OnPropertyChangedWithValue(value, "ItemCount");
                }
            }
        }

        [DataSourceProperty]
        public int ItemCost
        {
            get { return item.EquipmentElement.ItemValue; }
        }

        public ItemRosterElement item { get; private set; }
        public bool IsInSlot { get; private set; } = false;

        public EnchantmentItemVM() { }

        public EnchantmentItemVM(ItemRosterElement item)
        {
            this.item = item;
            ItemCount = item.Amount;
            ImageIdentifier = new ImageIdentifierVM(item.EquipmentElement.Item);
        }

        public EnchantmentItemVM SplitForUse()
        {
            ItemCount -= 1;
            return new EnchantmentItemVM(item) { ItemCount = 1, IsInSlot = true };
        }

        public bool HasSameItem(EnchantmentItemVM item)
        {
            return this.item.EquipmentElement.Item == item.item.EquipmentElement.Item;
        }
    }
}
