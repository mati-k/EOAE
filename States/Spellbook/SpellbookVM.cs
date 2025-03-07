using EOAE_Code.Data.Managers;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace EOAE_Code.States.Spellbook
{
    public class SpellbookVM : ViewModel
    {
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

        private MBBindingList<KnownSpellVM> _knownSpellList = new();

        [DataSourceProperty]
        public MBBindingList<KnownSpellVM> KnownSpellList
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

            foreach (var spell in SpellManager.GetAllSpell())
            {
                KnownSpellVM item = new KnownSpellVM(spell);
                KnownSpellList.Add(item);
            }
        }
    }
}
