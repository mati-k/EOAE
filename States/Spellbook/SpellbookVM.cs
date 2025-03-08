using EOAE_Code.Data.Managers;
using EOAE_Code.Magic.Spells;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace EOAE_Code.States.Spellbook
{
    public class SpellbookVM : ViewModel
    {
        private const int SPELL_SLOTS = 5;

        private string doneText = GameTexts.FindText("str_done").ToString();

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

        private MBBindingList<SpellSlotVM> _knownSpellList = new();

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

        private MBBindingList<SpellSlotVM> _pickedSpellList = new();

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

        public SpellbookVM()
        {
            Refresh();
        }

        public void ExecuteClose()
        {
            Game.Current.GameStateManager.PopState();
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

            foreach (var spell in SpellManager.GetAllSpell())
            {
                SpellSlotVM item = new SpellSlotVM(DropOnKnownSpell, false, spell);
                KnownSpellList.Add(item);
            }

            for (int i = 0; i < SPELL_SLOTS; i++)
            {
                PickedSpellList.Add(new SpellSlotVM(DropOnPickedSpell, true));
            }
        }

        private void DropOnKnownSpell(SpellSlotVM source, SpellSlotVM target)
        {
            if (!source.IsPickedList)
            {
                return;
            }

            source.ChangeSpell(null);
        }

        private void DropOnPickedSpell(SpellSlotVM source, SpellSlotVM target)
        {
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
    }
}
