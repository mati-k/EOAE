using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
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
        private HintViewModel _hint;

        private SPItemVM _itemSlot;
        private SPItemVM _enchantmentSlot;

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
        public SPItemVM ItemSlot
        {
            get { return this._itemSlot; }
            set
            {
                if (value != this._itemSlot)
                {
                    this._itemSlot = value;
                    base.OnPropertyChangedWithValue<SPItemVM>(value, "ItemSlot");
                }
            }
        }

        [DataSourceProperty]
        public SPItemVM EnchantmentSlot
        {
            get { return this._enchantmentSlot; }
            set
            {
                if (value != this._enchantmentSlot)
                {
                    this._enchantmentSlot = value;
                    base.OnPropertyChangedWithValue<SPItemVM>(value, "EnchantmentSlot");
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

            Hint = new HintViewModel(new TaleWorlds.Localization.TextObject("Some hint"));

            ItemSlot = new SPItemVM();
            EnchantmentSlot = new SPItemVM();
        }
    }
}
