using System;
using System.Collections.Generic;
using EOAE_Code.Magic.Spells;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace EOAE_Code.States.Spellbook
{
    public class SpellSlotVM : ViewModel
    {
        public Spell? Spell { get; private set; }
        public bool IsPickedList { get; private set; }

        private Action<SpellSlotVM, SpellSlotVM> onDrop;

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

        public SpellSlotVM(
            Action<SpellSlotVM, SpellSlotVM> onDrop,
            bool isPickedList,
            Spell? spell = null
        )
        {
            this.onDrop = onDrop;
            IsPickedList = isPickedList;
            Spell = spell;

            UpdateImageIdentifier();
        }

        private void UpdateImageIdentifier()
        {
            if (Spell != null)
            {
                ImageIdentifier = new ImageIdentifierVM(
                    MBObjectManager.Instance.GetObject<ItemObject>(Spell.ItemName)
                );
            }
            else
            {
                ImageIdentifier = new ImageIdentifierVM();
            }
        }

        public void ExecuteBeginHint()
        {
            if (Spell == null)
                return;

            List<TooltipProperty> tooltips = new();

            tooltips.Add(
                new TooltipProperty(
                    Spell.Name,
                    "",
                    0,
                    false,
                    TooltipProperty.TooltipPropertyFlags.Title
                )
            );
            tooltips.Add(
                new TooltipProperty(
                    new TextObject("{=b4rm2mLd}Cost").ToString(),
                    Spell.Cost.ToString(),
                    0
                )
            );
            tooltips.Add(
                new TooltipProperty(
                    new TextObject("{=ZyJ3GWMi}School").ToString(),
                    Spell.School.Name.ToString(),
                    0
                )
            );

            InformationManager.ShowTooltip(typeof(List<TooltipProperty>), tooltips);
        }

        public void ExecuteEndHint()
        {
            MBInformationManager.HideInformations();
        }

        public void ExecuteTransferWithParameters(SpellSlotVM draggedSpellVM, int index)
        {
            onDrop(draggedSpellVM, this);
        }

        public void ChangeSpell(Spell? spell)
        {
            Spell = spell;
            this.UpdateImageIdentifier();
        }
    }
}
