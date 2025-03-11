using EOAE_Code.Data.Managers;
using EOAE_Code.Magic.Spells;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace EOAE_Code.States.Spellbook
{
    public class SpellbookVM : ViewModel
    {
        private const int SPELL_SLOTS = 5;

        private string doneText = GameTexts.FindText("str_done").ToString();

        private CharacterSwitcherVM _characterSwitcher;
        private MBBindingList<SpellSlotVM> _knownSpellList = new();
        private MBBindingList<SpellSlotVM> _pickedSpellList = new();
        private InputKeyItemVM _doneInputKey;

        [DataSourceProperty]
        public CharacterSwitcherVM CharacterSwitcher
        {
            get { return _characterSwitcher; }
            set
            {
                if (value != _characterSwitcher)
                {
                    _characterSwitcher = value;
                    OnPropertyChangedWithValue(value, "CharacterSwitcher");
                }
            }
        }

        [DataSourceProperty]
        public string DoneText
        {
            get { return doneText; }
            set
            {
                if (value != doneText)
                {
                    doneText = value;
                    OnPropertyChangedWithValue(value, "DoneText");
                }
            }
        }

        [DataSourceProperty]
        public MBBindingList<SpellSlotVM> KnownSpellList
        {
            get { return _knownSpellList; }
            set
            {
                if (value != _knownSpellList)
                {
                    _knownSpellList = value;
                    OnPropertyChangedWithValue(value, "KnownSpellList");
                }
            }
        }

        [DataSourceProperty]
        public MBBindingList<SpellSlotVM> PickedSpellList
        {
            get { return _pickedSpellList; }
            set
            {
                if (value != _pickedSpellList)
                {
                    _pickedSpellList = value;
                    OnPropertyChangedWithValue(value, "PickedSpellList");
                }
            }
        }

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

        public SpellbookVM()
        {
            CharacterSwitcher = new CharacterSwitcherVM();

            Refresh();
        }

        public void ExecuteClose()
        {
            Game.Current.GameStateManager.PopState();

            // todo: Save
        }

        public void SetDoneInputKey(HotKey hotKey)
        {
            this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            Refresh();
        }

        private void Refresh()
        {
            KnownSpellList.Clear();
            PickedSpellList.Clear();

            for (int i = 0; i < 30; i++)
            {
                foreach (var spell in SpellManager.GetAllSpell())
                {
                    SpellSlotVM item = new SpellSlotVM(DropOnKnownSpell, false, spell);
                    KnownSpellList.Add(item);
                }
            }

            for (int i = 0; i < SPELL_SLOTS; i++)
            {
                PickedSpellList.Add(new SpellSlotVM(DropOnPickedSpell, true));
            }
        }

        public void ExecuteDropOnGrid(SpellSlotDraggableImageVM source, int index)
        {
            if (!source.Parent.IsPickedList)
            {
                source.Parent.ChangeSpell(null);
            }
        }

        private void DropOnKnownSpell(SpellSlotVM source, SpellSlotVM target)
        {
            if (source.IsPickedList)
            {
                source.ChangeSpell(null);
            }
        }

        private void DropOnPickedSpell(SpellSlotVM source, SpellSlotVM target)
        {
            if (source.Spell == null)
            {
                return;
            }

            if (source.IsPickedList)
            {
                Spell? swap = target.Spell;

                target.ChangeSpell(source.Spell);
                source.ChangeSpell(swap);
            }
            else
            {
                target.ChangeSpell(source.Spell);
            }
        }

        public override void OnFinalize()
        {
            base.OnFinalize();

            DoneInputKey.OnFinalize();
            CharacterSwitcher.OnFinalize();
        }
    }
}
