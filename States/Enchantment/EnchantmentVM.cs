using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace EOAE_Code.States.Enchantment
{
    public class EnchantmentVM : ViewModel
    {
        private string _doneText = GameTexts.FindText("str_done").ToString();
        private InputKeyItemVM _doneInputKey;
        private MBBindingList<EnchantmentItemVM> _itemList = new();

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
        }
    }
}
