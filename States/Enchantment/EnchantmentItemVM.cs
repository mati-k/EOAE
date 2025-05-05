using TaleWorlds.Core;
using TaleWorlds.Library;

namespace EOAE_Code.States.Enchantment
{
    public class EnchantmentItemVM : EnchantmentDraggable
    {
        private int _itemCount;

        [DataSourceProperty]
        public override string Name
        {
            get { return Item?.EquipmentElement.GetModifiedItemName().ToString() ?? string.Empty; }
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
            get { return Item?.EquipmentElement.ItemValue ?? 0; }
        }

        public ItemRosterElement? Item { get; private set; }

        public bool IsFiltered => throw new System.NotImplementedException();

        public EnchantmentItemVM() { }

        public EnchantmentItemVM(bool isInSlot)
        {
            IsInSlot = isInSlot;
            ImageIdentifier = new ImageIdentifierVM();
        }

        public EnchantmentItemVM(ItemRosterElement item)
        {
            this.Item = item;
            ItemCount = item.Amount;
            ImageIdentifier = new ImageIdentifierVM(item.EquipmentElement.Item);
        }

        public void AssignToSlot(EnchantmentItemVM item)
        {
            this.Item = item.Item;
            this.ImageIdentifier = item.ImageIdentifier;
            this.ItemCount = 1;

            item.ItemCount -= 1;
        }

        public bool HasSameItem(EnchantmentItemVM item)
        {
            if (item.Item == null && this.Item == null)
                return true;

            if (item.Item == null || this.Item == null)
                return false;

            return this.Item.Value.EquipmentElement.Item == item.Item.Value.EquipmentElement.Item;
        }

        public override void Clear()
        {
            this.Item = null;
            this.ImageIdentifier = new ImageIdentifierVM();
        }
    }
}
