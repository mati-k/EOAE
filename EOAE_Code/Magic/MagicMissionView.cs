using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace EOAE_Code.Magic
{
    [DefaultView]
    public class MagicMissionView : MissionView
    {
        private MagicHudVM? magicHUD;
        private GauntletLayer magicLayer;

        public override void OnBehaviorInitialize()
        {
            base.OnBehaviorInitialize();

            magicHUD = new MagicHudVM();
            magicLayer = new GauntletLayer(0);
            magicLayer.LoadMovie("MagicHUD", magicHUD);
            MissionScreen.AddLayer(magicLayer);
            magicHUD.Initialize();
        }

        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);

            magicHUD?.Tick();
        }
    }
}
