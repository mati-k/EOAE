﻿using System.ComponentModel;
using EOAE_Code.Agents;
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

        private int _agentMagicMax = 100;

        [DataSourceProperty]
        public int AgentMagicMax
        {
            get => _agentMagicMax;
            set
            {
                if (_agentMagicMax != value)
                {
                    _agentMagicMax = value;
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
            var equippedSpell = Agent.Main?.GetEquippedSpell();

            SpellInfo = equippedSpell != null ? $"{equippedSpell.Name} ({equippedSpell.Cost})" : "";
        }

        public void Tick()
        {
            if (Agent.Main != null)
            {
                var agentMana = MagicMissionLogic.GetAgentMana(Agent.Main);

                if (agentMana == null)
                {
                    return;
                }

                AgentMagic = (int)agentMana.CurrentMana;
                AgentMagicMax = (int)agentMana.MaxMana;
            }
        }
    }
}
