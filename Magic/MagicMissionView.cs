using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View;

namespace EOAE_Code.Magic
{
    [DefaultView]
    public class MagicMissionView : MissionView
    {
        private MagicHudVM magicHUD;
        private GauntletLayer magicLayer;

        public override void OnBehaviorInitialize()
        {
            base.OnBehaviorInitialize();

            magicHUD = new MagicHudVM(this.Mission);
            magicLayer = new GauntletLayer(0);
            magicLayer.LoadMovie("MagicHUD", magicHUD);
            MissionScreen.AddLayer(magicLayer);
        }

        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);

            if (magicHUD != null)
            {
                magicHUD.Tick();
            }
        }
    }
}
