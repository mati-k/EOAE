using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace EOAE_Code.States.Spellbook
{
    public class SpellSlotDraggableImageVM : ViewModel
    {
        public SpellSlotVM Parent { get; private set; }

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

        public SpellSlotDraggableImageVM(SpellSlotVM parent)
        {
            Parent = parent;
            UpdateImageIdentifier();
        }

        public void UpdateImageIdentifier()
        {
            if (Parent.Spell != null)
            {
                ImageIdentifier = new ImageIdentifierVM(
                    MBObjectManager.Instance.GetObject<ItemObject>(Parent.Spell.ItemName)
                );
            }
            else
            {
                ImageIdentifier = new ImageIdentifierVM();
            }
        }
    }
}
