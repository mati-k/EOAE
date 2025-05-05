using EOAE_Code.Data.Managers;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace EOAE_Code.States.Enchantment
{
    public class EnchantmentVM : ViewModel
    {
        private string _doneText = GameTexts.FindText("str_done").ToString();
        private InputKeyItemVM _doneInputKey;
        private MBBindingList<EnchantmentItemVM> _itemList = new();
        private MBBindingList<EnchantmentEnchantmentVM> _enchantmentList = new();
        private HintViewModel _hint;

        private EnchantmentSlotVM<EnchantmentItemVM> _itemSlot;
        private EnchantmentSlotVM<EnchantmentEnchantmentVM> _enchantmentSlot;

        [DataSourceProperty]
        public InputKeyItemVM DoneInputKey
        {
            get { return this._doneInputKey; }
            set
            {
                if (value != this._doneInputKey)
                {
                    this._doneInputKey = value;
                    base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
                }
            }
        }

        [DataSourceProperty]
        public string DoneText
        {
            get { return _doneText; }
            set
            {
                if (value != _doneText)
                {
                    _doneText = value;
                    OnPropertyChangedWithValue(value, "DoneText");
                }
            }
        }

        [DataSourceProperty]
        public MBBindingList<EnchantmentItemVM> ItemList
        {
            get { return this._itemList; }
            set
            {
                if (value != this._itemList)
                {
                    this._itemList = value;
                    base.OnPropertyChangedWithValue<MBBindingList<EnchantmentItemVM>>(
                        value,
                        "ItemList"
                    );
                }
            }
        }

        [DataSourceProperty]
        public MBBindingList<EnchantmentEnchantmentVM> EnchantmentList
        {
            get { return this._enchantmentList; }
            set
            {
                if (value != this._enchantmentList)
                {
                    this._enchantmentList = value;
                    base.OnPropertyChangedWithValue<MBBindingList<EnchantmentEnchantmentVM>>(
                        value,
                        "EnchantmentList"
                    );
                }
            }
        }

        [DataSourceProperty]
        public HintViewModel Hint
        {
            get { return this._hint; }
            set
            {
                if (value != this._hint)
                {
                    this._hint = value;
                    base.OnPropertyChangedWithValue<HintViewModel>(value, "Hint");
                }
            }
        }

        [DataSourceProperty]
        public EnchantmentSlotVM<EnchantmentItemVM> ItemSlot
        {
            get { return this._itemSlot; }
            set
            {
                if (value != this._itemSlot)
                {
                    this._itemSlot = value;
                    base.OnPropertyChangedWithValue<EnchantmentSlotVM<EnchantmentItemVM>>(
                        value,
                        "ItemSlot"
                    );
                }
            }
        }

        [DataSourceProperty]
        public EnchantmentSlotVM<EnchantmentEnchantmentVM> EnchantmentSlot
        {
            get { return this._enchantmentSlot; }
            set
            {
                if (value != this._enchantmentSlot)
                {
                    this._enchantmentSlot = value;
                    base.OnPropertyChangedWithValue<EnchantmentSlotVM<EnchantmentEnchantmentVM>>(
                        value,
                        "EnchantmentSlot"
                    );
                }
            }
        }

        [DataSourceProperty]
        public int EnchantmentValueMin { get; } = 5;

        [DataSourceProperty]
        public int EnchantmentValueMax { get; } = 25;

        private int _enchantmentValue = 10;

        [DataSourceProperty]
        public int EnchantmentValue
        {
            get { return _enchantmentValue; }
            set
            {
                if (value != _enchantmentValue)
                {
                    _enchantmentValue = value;
                    OnPropertyChangedWithValue(value, "EnchantmentValue");
                }
            }
        }

        public EnchantmentVM()
            : base()
        {
            Refresh();
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            Refresh();
        }

        public void SetDoneInputKey(HotKey hotKey)
        {
            this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
        }

        public void ExecuteClose()
        {
            Game.Current.GameStateManager.PopState();
        }

        public override void OnFinalize()
        {
            base.OnFinalize();

            DoneInputKey.OnFinalize();
        }

        private void Refresh()
        {
            var inventory = MobileParty.MainParty.ItemRoster;
            foreach (var item in inventory)
            {
                var enchantmentItemVM = new EnchantmentItemVM(item);
                ItemList.Add(enchantmentItemVM);
            }

            var enchantments = EnchantmentManager.GetAllEnchantments();
            foreach (var enchantment in enchantments)
            {
                var enchantmentItemVM = new EnchantmentEnchantmentVM(enchantment);
                EnchantmentList.Add(enchantmentItemVM);
            }

            Hint = new HintViewModel(new TaleWorlds.Localization.TextObject("Some hint"));

            ItemSlot = new EnchantmentSlotVM<EnchantmentItemVM>(new EnchantmentItemVM(true));
            EnchantmentSlot = new EnchantmentSlotVM<EnchantmentEnchantmentVM>(
                new EnchantmentEnchantmentVM(true)
            );
        }

        public void ExecuteDropOnEnchantingArea(EnchantmentDraggable draggable, int index)
        {
            if (draggable.IsInSlot)
            {
                return;
            }

            if (draggable is EnchantmentItemVM item)
            {
                if (!ItemSlot.IsEmpty())
                {
                    ReturnItemToInventory(ItemSlot.Item);
                }

                ItemSlot.Item.AssignToSlot(item);

                for (int i = 0; i < ItemList.Count; i++)
                {
                    if (ItemList[i].ItemCount == 0)
                    {
                        ItemList.RemoveAt(i);
                        break;
                    }
                }
            }
            else if (draggable is EnchantmentEnchantmentVM enchantment)
            {
                EnchantmentSlot.Item.AssignToSlot(enchantment);
            }
        }

        public void ExecuteDropOnList(EnchantmentDraggable draggable, int index)
        {
            if (!draggable.IsInSlot)
            {
                return;
            }

            if (draggable is EnchantmentItemVM item)
            {
                ReturnItemToInventory(item);
            }

            draggable.Clear();
        }

        public void DropOnInventory(EnchantmentItemVM item, int index)
        {
            if (item.IsInSlot)
            {
                ReturnItemToInventory(item);
            }
        }

        private void ReturnItemToInventory(EnchantmentItemVM item)
        {
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (ItemList[i].HasSameItem(item))
                {
                    ItemList[i].ItemCount += item.ItemCount;
                    return;
                }
            }

            ItemList.Add(new EnchantmentItemVM(item.Item!.Value));
        }
    }
}
