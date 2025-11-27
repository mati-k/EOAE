using EOAE_Code.Data.Xml.Enchantments;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

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
            ImageIdentifier = new GenericImageIdentifierVM(null);
        }

        public EnchantmentEnchantmentVM(EnchantmentData enchantment)
        {
            this.EnchantmentData = enchantment;
            ImageIdentifier = new ItemImageIdentifierVM(
                MBObjectManager.Instance.GetObject<ItemObject>(enchantment.IconItem)
            );
        }

        public void AssignToSlot(EnchantmentEnchantmentVM enchantment)
        {
            this.ImageIdentifier = enchantment.ImageIdentifier;
            this.EnchantmentData = enchantment.EnchantmentData;
        }

        public override void Clear()
        {
            this.ImageIdentifier = new GenericImageIdentifierVM(null);
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
