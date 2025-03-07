using System.Collections.Generic;
using EOAE_Code.Magic.Spells;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace EOAE_Code.States.Spellbook
{
    public class KnownSpellVM : ViewModel
    {
        private Spell spell;

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

        public KnownSpellVM(Spell spell)
        {
            this.spell = spell;

            ImageIdentifier = new ImageIdentifierVM(
                MBObjectManager.Instance.GetObject<ItemObject>(spell.ItemName)
            );
        }

        public void ExecuteBeginHint()
        {
            List<TooltipProperty> tooltips = new();

            tooltips.Add(
                new TooltipProperty(
                    spell.Name,
                    "",
                    0,
                    false,
                    TooltipProperty.TooltipPropertyFlags.Title
                )
            );
            tooltips.Add(
                new TooltipProperty(
                    new TextObject("{=b4rm2mLd}Cost").ToString(),
                    spell.Cost.ToString(),
                    0
                )
            );
            tooltips.Add(
                new TooltipProperty(
                    new TextObject("{=ZyJ3GWMi}School").ToString(),
                    spell.School.Name.ToString(),
                    0
                )
            );

            InformationManager.ShowTooltip(typeof(List<TooltipProperty>), tooltips);
        }

        public void ExecuteEndHint()
        {
            MBInformationManager.HideInformations();
        }
    }
}
