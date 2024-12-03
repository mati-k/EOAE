using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic
{
    public class MagicHudVM : ViewModel
    {
        public MagicHudVM(Mission mission)
        {
            this.RefreshValues();
        }

        public void Tick()
        {

        }

        [DataSourceProperty]
        public int AgentMagic { get { return 60; } }
        [DataSourceProperty]
        public int AgentMagicMax { get { return 100; } }
        [DataSourceProperty]
        public bool ShowMagicHealthBar { get { return true; } }
    }
}
