using TaleWorlds.Library;

namespace EOAE_Code.States.Enchantment
{
    public class EnchantmentSlotVM<T> : ViewModel
        where T : EnchantmentDraggable, new()
    {
        private T _item = new();

        [DataSourceProperty]
        public T Item
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

        public EnchantmentSlotVM(T item)
        {
            Item = item;
        }

        public bool IsEmpty()
        {
            return !Item.IsImageSet;
        }
    }
}
