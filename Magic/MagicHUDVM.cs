using System.ComponentModel;
using EOAE_Code.Data.Managers;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic
{
    public class MagicHudVM : ViewModel
    {
        private int _agentMagic;

        [DataSourceProperty]
        public int AgentMagic
        {
            get => _agentMagic;
            set
            {
                if (_agentMagic != value)
                {
                    _agentMagic = value;
                    OnPropertyChangedWithValue(value);
                }
            }
        }

        private string _spellInfo;

        [DataSourceProperty]
        public string SpellInfo
        {
            get => _spellInfo;
            set
            {
                if (_spellInfo != value)
                {
                    _spellInfo = value;
                    OnPropertyChangedWithValue(value);
                }
            }
        }

        [DataSourceProperty]
        public int AgentMagicMax => 100;

        [DataSourceProperty]
        public bool ShowMagicHealthBar => true;

        public void Initialize()
        {
            Mission.Current.OnMainAgentChanged += OnMainAgentChanged;
            OnMainAgentChanged(null, null);
        }

        private void OnMainAgentChanged(object? sender, PropertyChangedEventArgs? e)
        {
            if (Agent.Main != null)
            {
                Agent.Main.OnMainAgentWieldedItemChange += OnMainAgentWieldedItemChange;
                OnMainAgentWieldedItemChange();
            }
        }

        private void OnMainAgentWieldedItemChange()
        {
            var itemName = Agent.Main.WieldedWeapon.Item?.StringId;
            if (itemName != null && SpellManager.IsSpell(itemName))
            {
                var spell = SpellManager.GetSpellFromItem(itemName);
                SpellInfo = $"{spell.Name} ({spell.Cost})";
            }
            else
            {
                SpellInfo = "";
            }
        }

        public void Tick()
        {
            if (Agent.Main != null && MagicMissionLogic.CurrentMana.ContainsKey(Agent.Main))
            {
                AgentMagic = (int)MagicMissionLogic.CurrentMana[Agent.Main];
            }
        }
    }
}
