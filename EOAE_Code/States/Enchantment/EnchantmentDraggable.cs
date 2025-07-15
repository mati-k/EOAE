using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace EOAE_Code.States.Enchantment
{
    public abstract class EnchantmentDraggable : ViewModel
    {
        private ImageIdentifierVM _imageIdentifier;

        public event EventHandler ItemChanged;

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
                    base.OnPropertyChanged("IsImageSet");
                }
            }
        }

        [DataSourceProperty]
        public bool IsImageSet
        {
            get { return ImageIdentifier != null && ImageIdentifier.Id != ""; }
        }

        protected void OnItemChanged()
        {
            ItemChanged?.Invoke(this, EventArgs.Empty);
        }

        public abstract string Name { get; }
        public bool IsInSlot { get; protected set; }

        public abstract void Clear();
    }
}
