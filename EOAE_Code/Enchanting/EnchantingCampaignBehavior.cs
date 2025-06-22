using System.Collections.Generic;
using EOAE_Code.States.Enchantment;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace EOAE_Code.Enchanting
{
    public class EnchantingCampaignBehavior : CampaignBehaviorBase
    {
        private Dictionary<ItemObject, EnchantedItem> enchantedItems = new();

        public override void RegisterEvents()
        {
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, Initialize);
        }

        public override void SyncData(IDataStore dataStore)
        {
            // Idea: No need to create new ItemObject instances for enchanting and just bind to ItemRosterElement.
            // Todo: Functional issue: ItemRosterElement is struct, so using would be problematic, to investigate in future

            dataStore.SyncData<Dictionary<ItemObject, EnchantedItem>>(
                "_enchantedItems",
                ref enchantedItems
            );

            if (dataStore.IsLoading)
            {
                foreach (var item in enchantedItems)
                {
                    item.Value.LoadEnchantment();
                }
            }
        }

        private void Initialize(CampaignGameStarter starter)
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

        public void RegisterEnchantedItem(ItemObject item, EnchantedItem enchantedItem)
        {
            if (item == null || enchantedItem == null)
            {
                return;
            }

            enchantedItems[item] = enchantedItem;
        }

        public bool IsItemEnchanted(ItemObject item)
        {
            return enchantedItems.ContainsKey(item);
        }

        public EnchantedItem? GetItemEnchantment(ItemObject item)
        {
            return enchantedItems.TryGetValue(item, out var enchantedItem) ? enchantedItem : null;
        }
    }
}
