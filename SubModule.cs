using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;


namespace EOAE_Code
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();

        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

        }

        protected override void InitializeGameStarter(Game game, IGameStarter starterObject)
        {
            if (Game.Current.GameType is Campaign && starterObject is CampaignGameStarter)
            {
                var starter = starterObject as CampaignGameStarter;
                starter.AddBehavior(new SavePatch());
            }
        }
    }
}