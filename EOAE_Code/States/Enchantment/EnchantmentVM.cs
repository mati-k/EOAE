using System.Linq;
using EOAE_Code.Consts;
using EOAE_Code.Data.Managers;
using EOAE_Code.Enchanting;
using EOAE_Code.Extensions;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

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

        private string _doneText = new TextObject("{=riOS7Pil}Enchant").ToString();
        private InputKeyItemVM _doneInputKey;
        private MBBindingList<EnchantmentItemVM> _itemList = new();
        private MBBindingList<EnchantmentEnchantmentVM> _enchantmentList = new();
        private MBBindingList<EnchantmentSoulGemVM> _soulGemList = new();
        private HintViewModel _hint;

        private EnchantmentSlotVM<EnchantmentItemVM> _itemSlot;
        private EnchantmentSlotVM<EnchantmentEnchantmentVM> _enchantmentSlot;
        private EnchantmentSlotVM<EnchantmentSoulGemVM> _soulGemSlot;

        private string _enchantmentDescription = string.Empty;
        private string _enchantmentItemName = "";

        [DataSourceProperty]
        public InputKeyItemVM DoneInputKey
        {
            get { return this._doneInputKey; }
            set
            {
                if (value != this._doneInputKey)
                {
                    this._doneInputKey = value;
                    base.OnPropertyChangedWithValue<InputKeyItemVM>(value, nameof(DoneInputKey));
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
                    OnPropertyChangedWithValue(value, nameof(DoneText));
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
                        nameof(ItemList)
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
                        nameof(EnchantmentList)
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
                        nameof(SoulGemList)
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
                    base.OnPropertyChangedWithValue<HintViewModel>(value, nameof(Hint));
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
                        nameof(ItemSlot)
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
                        nameof(EnchantmentSlot)
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
                        nameof(SoulGemSlot)
                    );
                }
            }
        }

        [DataSourceProperty]
        public bool CanFinish
        {
            get
            {
                return !ItemSlot.IsEmpty() && !EnchantmentSlot.IsEmpty() && !SoulGemSlot.IsEmpty();
            }
        }

        [DataSourceProperty]
        public int ScaleSliderMin { get; } = 0;

        [DataSourceProperty]
        public int ScaleSliderMax { get; } = EnchantmentConsts.SliderScaleSteps;

        private int _scaleSliderValue = 1;

        [DataSourceProperty]
        public int ScaleSliderValue
        {
            get { return _scaleSliderValue; }
            set
            {
                if (value != _scaleSliderValue)
                {
                    _scaleSliderValue = value;
                    OnPropertyChangedWithValue(value, nameof(ScaleSliderValue));
                    RefreshEnchantmentDescription();
                }
            }
        }

        public float EnchantmentScale
        {
            get
            {
                float scale =
                    EnchantmentConsts.SliderScaleMin
                    + (EnchantmentConsts.SliderScaleMax - EnchantmentConsts.SliderScaleMin)
                        * ((float)ScaleSliderValue / EnchantmentConsts.SliderScaleSteps);
                return MathF.Round(scale, 1);
            }
        }

        [DataSourceProperty]
        public string EnchantmentDescription
        {
            get { return _enchantmentDescription; }
            set
            {
                if (value != _enchantmentDescription)
                {
                    _enchantmentDescription = value;
                    OnPropertyChangedWithValue(value, nameof(EnchantmentDescription));
                }
            }
        }

        [DataSourceProperty]
        public string EnchantmentItemName
        {
            get { return _enchantmentItemName; }
            set
            {
                if (value != _enchantmentItemName)
                {
                    _enchantmentItemName = value;
                    OnPropertyChangedWithValue(value, nameof(EnchantmentItemName));
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

        public void ExecuteEnchantVariant()
        {
            var item = ItemSlot.Item.Item;
            var enchantment = EnchantmentSlot.Item.EnchantmentData;
            var soulGem = SoulGemSlot.Item.Item;
            if (item == null || enchantment == null || soulGem == null)
            {
                return;
            }

            var enchantmentModifier = new ItemModifier() { StringId = "ENCHANTMENT_1" };
            AccessTools
                .DeclaredPropertySetter(typeof(ItemModifier), "Name")
                .Invoke(
                    enchantmentModifier,
                    new object[] { new TextObject("{=!}" + EnchantmentItemName) }
                );

            MBObjectManager.Instance.RegisterObject<ItemModifier>(enchantmentModifier);
            enchantmentModifier.Initialize();

            var equipmentElement = item.Value.EquipmentElement;
            EquipmentElement enchantedEquipmentElement = new EquipmentElement(
                equipmentElement.Item,
                enchantmentModifier,
                equipmentElement.CosmeticItem,
                equipmentElement.IsQuestItem
            );

            PartyBase.MainParty.ItemRoster.AddToCounts(enchantedEquipmentElement, 1);
            PartyBase.MainParty.ItemRoster.AddToCounts(equipmentElement, -1);
        }

        public void ExecuteEnchant()
        {
            var item = ItemSlot.Item.Item;
            var enchantment = EnchantmentSlot.Item.EnchantmentData;
            var soulGem = SoulGemSlot.Item.Item;
            if (item == null || enchantment == null || soulGem == null)
            {
                return;
            }

            var equipmentElement = item.Value.EquipmentElement;

            ItemObject enchantedItem = new ItemObject(equipmentElement.Item);
            ItemObject.InitAsPlayerCraftedItem(ref enchantedItem);
            enchantedItem.StringId =
                $"{equipmentElement.Item.StringId}_{enchantment.Name}_{EnchantmentScale.ToString("F1")}";

            if (equipmentElement.Item.WeaponDesign != null)
            {
                AccessTools
                    .DeclaredPropertySetter(typeof(ItemObject), "WeaponDesign")
                    .Invoke(enchantedItem, new object[] { equipmentElement.Item.WeaponDesign });
            }

            AccessTools
                .Method(typeof(ItemObject), "SetName")
                .Invoke(enchantedItem, new object[] { new TextObject(EnchantmentItemName) });
            AccessTools
                .DeclaredPropertySetter(typeof(ItemObject), "Culture")
                .Invoke(enchantedItem, new object[] { Hero.MainHero.Culture });

            EquipmentElement enchantedEquipmentElement = new EquipmentElement(
                enchantedItem,
                equipmentElement.ItemModifier,
                equipmentElement.CosmeticItem,
                equipmentElement.IsQuestItem
            );

            enchantedItem.DetermineItemCategoryForItem();

            MBObjectManager.Instance.RegisterObject<ItemObject>(enchantedItem);
            Campaign
                .Current.GetCampaignBehavior<EnchantingCampaignBehavior>()
                .RegisterEnchantedItem(
                    enchantedItem,
                    new EnchantedItem(enchantment, EnchantmentScale)
                );

            PartyBase.MainParty.ItemRoster.AddToCounts(enchantedItem, 1);
            PartyBase.MainParty.ItemRoster.AddToCounts(equipmentElement, -1);

            CampaignEventDispatcher.Instance.OnNewItemCrafted(enchantedItem, null, false);

            Refresh();
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
                if (
                    !item.EquipmentElement.Item.IsEnchanted()
                    && EnchantableTypes.Contains(item.EquipmentElement.Item.Type)
                )
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
            ItemSlot.Item.ItemChanged += OnItemChanged;

            EnchantmentSlot = new EnchantmentSlotVM<EnchantmentEnchantmentVM>(
                new EnchantmentEnchantmentVM(true)
            );
            EnchantmentSlot.Item.ItemChanged += OnEnchantmentChanged;

            SoulGemSlot = new EnchantmentSlotVM<EnchantmentSoulGemVM>(
                new EnchantmentSoulGemVM(true)
            );
            SoulGemSlot.Item.ItemChanged += OnSoulGemChanged;
        }

        private void OnEnchantmentChanged(object? sender, System.EventArgs e)
        {
            base.OnPropertyChanged(nameof(IsSliderVisible));
            base.OnPropertyChanged(nameof(CanFinish));
            UpdateItemFilters();
        }

        private void OnItemChanged(object? sender, System.EventArgs e)
        {
            base.OnPropertyChanged(nameof(CanFinish));
            if (ItemSlot.Item.Item != null)
            {
                EnchantmentItemName =
                    ItemSlot.Item.Item.Value.EquipmentElement.Item.Name.ToString();
            }
        }

        private void OnSoulGemChanged(object? sender, System.EventArgs e)
        {
            base.OnPropertyChanged(nameof(CanFinish));
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
            if (EnchantmentSlot.IsEmpty())
            {
                EnchantmentDescription = string.Empty;
                return;
            }

            float value = 1f;

            if (IsSliderVisible)
            {
                value = EnchantmentScale;
            }
            else if (SoulGemSlot.Item.Item != null)
            {
                value *= EnchantmentManager.GetSoulGemValue(SoulGemSlot.Item.Item.StringId);
            }

            EnchantmentDescription = EnchantmentSlot.Item.EnchantmentData.GetDescription(value);
        }
    }
}
