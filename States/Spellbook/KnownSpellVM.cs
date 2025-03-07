using EOAE_Code.Magic.Spells;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace EOAE_Code.States.Spellbook
{
    public class KnownSpellVM : ViewModel
    {
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
            ImageIdentifier = new ImageIdentifierVM(
                MBObjectManager.Instance.GetObject<ItemObject>(spell.ItemName)
            );
        }
    }
}
