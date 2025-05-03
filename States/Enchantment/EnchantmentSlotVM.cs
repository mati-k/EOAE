using TaleWorlds.Library;

namespace EOAE_Code.States.Enchantment
{
    public class EnchantmentSlotVM : ViewModel
    {
        private EnchantmentItemVM _item = new EnchantmentItemVM();

        [DataSourceProperty]
        public EnchantmentItemVM Item
        {
            get { return _item; }
            set
            {
                if (value != _item)
                {
                    _item = value;
                    OnPropertyChangedWithValue(value, "Item");
                }
            }
        }

        public void AssignItem(EnchantmentItemVM item)
        {
            Item = item;
        }

        public bool IsEmpty()
        {
            return Item == null || Item.ItemCount == 0;
        }
    }
}
