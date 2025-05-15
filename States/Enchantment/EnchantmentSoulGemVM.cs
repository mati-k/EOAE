using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace EOAE_Code.States.Enchantment
{
    // Todo: Replace crafting materials with gems items when added
    public class EnchantmentSoulGemVM : EnchantmentDraggable
    {
        private string _name;
        private string _stringId;
        private int _amount;
        private HintViewModel _soulGemHint;
        private ItemObject? item;

        [DataSourceProperty]
        public override string Name
        {
            get { return _name; }
        }

        [DataSourceProperty]
        public string StringId
        {
            get { return _stringId; }
            set
            {
                if (value != this._stringId)
                {
                    this._stringId = value;
                    base.OnPropertyChangedWithValue(value, "StringId");
                }
            }
        }

        [DataSourceProperty]
        public int Amount
        {
            get { return _amount; }
            set
            {
                if (value != this._amount)
                {
                    this._amount = value;
                    base.OnPropertyChangedWithValue(value, "Amount");
                    base.OnPropertyChanged("IsDraggable");
                }
            }
        }

        [DataSourceProperty]
        public bool IsDraggable
        {
            get => _amount > 0;
        }

        [DataSourceProperty]
        public HintViewModel SoulGemHint
        {
            get { return this._soulGemHint; }
            set
            {
                if (value != this._soulGemHint)
                {
                    this._soulGemHint = value;
                    base.OnPropertyChangedWithValue<HintViewModel>(value, "SoulGemHint");
                }
            }
        }

        public EnchantmentSoulGemVM() { }

        public EnchantmentSoulGemVM(bool isInSlot)
        {
            IsInSlot = isInSlot;
            ImageIdentifier = new ImageIdentifierVM();
        }

        public EnchantmentSoulGemVM(CraftingMaterials material, int amount)
        {
            Campaign campaign = Campaign.Current;
            if (campaign != null)
            {
                GameModels models = campaign.Models;
                if (models != null)
                {
                    SmithingModel smithingModel = models.SmithingModel;
                    item = (
                        (smithingModel != null)
                            ? smithingModel.GetCraftingMaterialItem(material)
                            : null
                    );
                }
            }

            if (item != null)
            {
                this.ImageIdentifier = new ImageIdentifierVM(item);
                this._soulGemHint = new HintViewModel(
                    new TextObject("{=!}" + item.Name.ToString(), null),
                    null
                );

                Amount = MobileParty.MainParty.ItemRoster.GetItemNumber(item);
                _name = item.Name.ToString();
            }

            this.StringId = material.ToString();
        }

        public bool HasSameItem(EnchantmentSoulGemVM soulGemVM)
        {
            return soulGemVM.item == this.item;
        }

        public override void Clear()
        {
            this.item = null;
            this.ImageIdentifier = new ImageIdentifierVM();
        }

        public void ExecuteStockDragBegin()
        {
            if (SoulGemHint != null)
            {
                SoulGemHint.ExecuteEndHint();
            }

            Amount--;
            // Make sure to keep it as draggable during the drag, because Command.DragEnd is not geting called otherwise
            OnPropertyChangedWithValue(true, "IsDraggable");
        }

        public void ExecuteStockDragEnd()
        {
            Amount++;
        }

        public void AssignToSlot(EnchantmentSoulGemVM soulGem)
        {
            this.item = soulGem.item;
            this.ImageIdentifier = soulGem.ImageIdentifier;
            this.Amount = 1;

            soulGem.Amount--;
        }
    }
}
