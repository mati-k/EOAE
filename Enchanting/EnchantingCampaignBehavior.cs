using EOAE_Code.States.Enchantment;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace EOAE_Code.Enchanting
{
    public class EnchantingCampaignBehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, Initialize);
        }

        public override void SyncData(IDataStore dataStore) { }

        public void Initialize(CampaignGameStarter starter)
        {
            starter.AddGameMenuOption(
                "town",
                "visit_enchanter",
                new TextObject("{=pERd4fn8}Go to Enchanter").ToString(),
                ShouldShowEnchantingMenu,
                OnEnchantingMenuClick
            );
        }

        private bool ShouldShowEnchantingMenu(MenuCallbackArgs args)
        {
            args.optionLeaveType = GameMenuOption.LeaveType.Craft;
            return true;
        }

        private void OnEnchantingMenuClick(MenuCallbackArgs args)
        {
            var manager = Game.Current.GameStateManager;
            manager.PushState(manager.CreateState<EnchantmentState>());
        }
    }
}
