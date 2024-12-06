using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic
{
    public class MagicHudVM : ViewModel
    {
        Mission _mission;

        private int _agentMagic;
        [DataSourceProperty]
        public int AgentMagic 
        { 
            get { return _agentMagic; } 
            set 
            {
                if (_agentMagic != value)
                {
                    _agentMagic = value;
                    base.OnPropertyChangedWithValue(value, "AgentMagic");
                }
            } 
        }
        [DataSourceProperty]
        public int AgentMagicMax { get { return 100; } }
        [DataSourceProperty]
        public bool ShowMagicHealthBar { get { return true; } }

        public MagicHudVM(Mission mission)
        {
            this._mission = mission;
            this.RefreshValues();
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
