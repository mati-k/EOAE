using System;
using System.Collections.Generic;
using EOAE_Code.Magic.Spells;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace EOAE_Code.States.Spellbook
{
    public class SpellSlotVM : ViewModel
    {
        public Spell? Spell { get; private set; }
        public bool IsPickedList { get; private set; }

        private Action<SpellSlotVM, SpellSlotVM> onDrop;

        private SpellSlotDraggableImageVM _spellSlotDraggableImage;

        [DataSourceProperty]
        public SpellSlotDraggableImageVM SpellSlotDraggableImage
        {
            get { return this._spellSlotDraggableImage; }
            set
            {
                if (value != this._spellSlotDraggableImage)
                {
                    this._spellSlotDraggableImage = value;
                    base.OnPropertyChangedWithValue<SpellSlotDraggableImageVM>(
                        value,
                        "SpellSlotDraggableImage"
                    );
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

            SpellSlotDraggableImage = new SpellSlotDraggableImageVM(this);
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

            if (Spell is MissileSpell missile)
            {
                missile.Effect?.AddTooltips(tooltips);
            }

            InformationManager.ShowTooltip(typeof(List<TooltipProperty>), tooltips);
        }

        public void ExecuteEndHint()
        {
            MBInformationManager.HideInformations();
        }

        public void ExecuteDropOnSlot(SpellSlotDraggableImageVM draggedSpellVM, int index)
        {
            onDrop(draggedSpellVM.Parent, this);
        }

        public void ChangeSpell(Spell? spell)
        {
            Spell = spell;
            SpellSlotDraggableImage.UpdateImageIdentifier();
        }
    }
}
