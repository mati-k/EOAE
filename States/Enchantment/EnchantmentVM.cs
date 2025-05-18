using System.Linq;
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
        private static readonly ItemObject.ItemTypeEnum[] EnchantableTypes =
            new ItemObject.ItemTypeEnum[]
            {
                ItemObject.ItemTypeEnum.OneHandedWeapon,
                ItemObject.ItemTypeEnum.TwoHandedWeapon,
                ItemObject.ItemTypeEnum.Polearm,
                ItemObject.ItemTypeEnum.Bow,
                ItemObject.ItemTypeEnum.Shield,
                ItemObject.ItemTypeEnum.Crossbow,
                ItemObject.ItemTypeEnum.HandArmor,
                ItemObject.ItemTypeEnum.HeadArmor,
                ItemObject.ItemTypeEnum.ChestArmor,
                ItemObject.ItemTypeEnum.LegArmor,
                ItemObject.ItemTypeEnum.Cape,
            };

        private string _doneText = GameTexts.FindText("str_done").ToString();
        private InputKeyItemVM _doneInputKey;
        private MBBindingList<EnchantmentItemVM> _itemList = new();
        private MBBindingList<EnchantmentEnchantmentVM> _enchantmentList = new();
        private MBBindingList<EnchantmentSoulGemVM> _soulGemList = new();
        private HintViewModel _hint;

        private EnchantmentSlotVM<EnchantmentItemVM> _itemSlot;
        private EnchantmentSlotVM<EnchantmentEnchantmentVM> _enchantmentSlot;
        private EnchantmentSlotVM<EnchantmentSoulGemVM> _soulGemSlot;

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
        public MBBindingList<EnchantmentSoulGemVM> SoulGemList
        {
            get { return this._soulGemList; }
            set
            {
                if (value != this._soulGemList)
                {
                    this._soulGemList = value;
                    base.OnPropertyChangedWithValue<MBBindingList<EnchantmentSoulGemVM>>(
                        value,
                        "SoulGemList"
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
        public EnchantmentSlotVM<EnchantmentSoulGemVM> SoulGemSlot
        {
            get { return this._soulGemSlot; }
            set
            {
                if (value != this._soulGemSlot)
                {
                    this._soulGemSlot = value;
                    base.OnPropertyChangedWithValue<EnchantmentSlotVM<EnchantmentSoulGemVM>>(
                        value,
                        "SoulGemSlot"
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

        private string _enchantmentDescription = string.Empty;

        [DataSourceProperty]
        public string EnchantmentDescription
        {
            get { return _enchantmentDescription; }
            set
            {
                if (value != _enchantmentDescription)
                {
                    _enchantmentDescription = value;
                    OnPropertyChangedWithValue(value, "EnchantmentDescription");
                }
            }
        }

        [DataSourceProperty]
        public bool IsSliderVisible
        {
            get { return EnchantmentSlot.Item.EnchantmentData?.UseSlider ?? false; }
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
            ItemList.Clear();
            var inventory = MobileParty.MainParty.ItemRoster;
            foreach (var item in inventory)
            {
                if (EnchantableTypes.Contains(item.EquipmentElement.Item.Type))
                {
                    var enchantmentItemVM = new EnchantmentItemVM(item);
                    ItemList.Add(enchantmentItemVM);
                }
            }

            EnchantmentList.Clear();
            var enchantments = EnchantmentManager.GetAllEnchantments();
            foreach (var enchantment in enchantments)
            {
                var enchantmentItemVM = new EnchantmentEnchantmentVM(enchantment);
                EnchantmentList.Add(enchantmentItemVM);
            }

            SoulGemList.Clear();
            for (int i = 0; i < 6; i++)
            {
                SoulGemList.Add(new EnchantmentSoulGemVM((CraftingMaterials)i, 0));
            }

            Hint = new HintViewModel(new TaleWorlds.Localization.TextObject("Some hint"));

            ItemSlot = new EnchantmentSlotVM<EnchantmentItemVM>(new EnchantmentItemVM(true));
            EnchantmentSlot = new EnchantmentSlotVM<EnchantmentEnchantmentVM>(
                new EnchantmentEnchantmentVM(true)
            );
            EnchantmentSlot.Item.ItemChanged += OnEnchantmentChanged;
            SoulGemSlot = new EnchantmentSlotVM<EnchantmentSoulGemVM>(
                new EnchantmentSoulGemVM(true)
            );
        }

        private void OnEnchantmentChanged(object? sender, System.EventArgs e)
        {
            base.OnPropertyChanged(nameof(IsSliderVisible));
            UpdateItemFilters();
        }

        public void ExecuteDropOnEnchantingArea(EnchantmentDraggable draggable, int index)
        {
            if (draggable is EnchantmentSoulGemVM soulGem)
            {
                if (draggable.IsInSlot)
                {
                    ReturnSoulGemToInventory(soulGem);
                    soulGem.Clear();
                }
                else if (!soulGem.HasSameItem(SoulGemSlot.Item))
                {
                    ReturnSoulGemToInventory(SoulGemSlot.Item);
                    SoulGemSlot.Item.AssignToSlot(soulGem);
                }
            }

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
                UpdateEnchantmentFilters();
            }
            else if (draggable is EnchantmentEnchantmentVM enchantment)
            {
                EnchantmentSlot.Item.AssignToSlot(enchantment);
            }

            RefreshEnchantmentDescription();
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
            else if (draggable is EnchantmentSoulGemVM soulGem)
            {
                ReturnSoulGemToInventory(soulGem);
            }

            draggable.Clear();
            UpdateEnchantmentFilters();
            RefreshEnchantmentDescription();
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

        private void ReturnSoulGemToInventory(EnchantmentSoulGemVM soulGemVM)
        {
            for (int i = 0; i < SoulGemList.Count; i++)
            {
                if (SoulGemList[i].HasSameItem(soulGemVM))
                {
                    SoulGemList[i].Amount += 1;
                    return;
                }
            }
        }

        private void UpdateItemFilters()
        {
            var enchantment = EnchantmentSlot.Item.EnchantmentData;

            for (int i = 0; i < ItemList.Count; i++)
            {
                ItemList[i].FilterToEnchantment(enchantment);
            }
            ItemSlot.Item.FilterToEnchantment(enchantment);
        }

        private void UpdateEnchantmentFilters()
        {
            var item = ItemSlot.Item.Item;
            for (int i = 0; i < EnchantmentList.Count; i++)
            {
                EnchantmentList[i].FilterToItem(item);
            }
            EnchantmentSlot.Item.FilterToItem(item);
        }

        private void RefreshEnchantmentDescription()
        {
            float value = 5;

            if (EnchantmentSlot.IsEmpty())
            {
                EnchantmentDescription = string.Empty;
                return;
            }

            EnchantmentDescription = EnchantmentSlot
                .Item.EnchantmentData.GetDescription(value)
                .ToString();
        }
    }
}
