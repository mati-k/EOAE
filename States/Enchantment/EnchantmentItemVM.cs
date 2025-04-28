using TaleWorlds.Core;
using TaleWorlds.Library;

namespace EOAE_Code.States.Enchantment
{
    public class EnchantmentItemVM : ViewModel
    {
        private ImageIdentifierVM _imageIdentifier;

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
            get { return item.Amount; }
        }

        [DataSourceProperty]
        public int ItemCost
        {
            get { return item.EquipmentElement.ItemValue; }
        }

        public ItemRosterElement item { get; private set; }

        public EnchantmentItemVM(ItemRosterElement item)
        {
            this.item = item;
            ImageIdentifier = new ImageIdentifierVM(item.EquipmentElement.Item);
        }
    }
}
