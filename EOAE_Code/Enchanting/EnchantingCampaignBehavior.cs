using System.Collections.Generic;
using EOAE_Code.States.Enchantment;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

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
                    SetupItemForEnchantment(item.Key, item.Value);
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

        public ItemObject RegisterEnchantedItem(string stringId, EnchantedItem enchantedItem)
        {
            if (enchantedItem == null || enchantedItem.BaseItem == null)
            {
                return new ItemObject();
            }

            var existing = MBObjectManager.Instance.GetObject<ItemObject>(stringId);
            if (existing != null && existing.Name.ToString() == enchantedItem.ItemName)
            {
                return existing;
            }

            ItemObject item = new ItemObject(stringId);

            SetupItemForEnchantment(item, enchantedItem);

            MBObjectManager.Instance.RegisterObject<ItemObject>(item);

            enchantedItems[item] = enchantedItem;
            enchantedItem.LoadEnchantment();

            return item;
        }

        private void SetupItemForEnchantment(ItemObject item, EnchantedItem enchantedItem)
        {
            ItemObject.InitAsPlayerCraftedItem(ref item);
            var itemToCopy = enchantedItem.BaseItem;

            AccessTools
                .Method(typeof(ItemObject), "SetName")
                .Invoke(item, new object[] { new TextObject(enchantedItem.ItemName) });
            AccessTools
                .DeclaredPropertySetter(typeof(ItemObject), "ItemComponent")
                .Invoke(item, new object[] { itemToCopy.ItemComponent });
            AccessTools
                .DeclaredPropertySetter(typeof(ItemObject), "MultiMeshName")
                .Invoke(item, new object[] { itemToCopy.MultiMeshName });
            AccessTools
                .DeclaredPropertySetter(typeof(ItemObject), "HolsterMeshName")
                .Invoke(item, new object[] { itemToCopy.HolsterMeshName });
            AccessTools
                .DeclaredPropertySetter(typeof(ItemObject), "HolsterWithWeaponMeshName")
                .Invoke(item, new object[] { itemToCopy.HolsterWithWeaponMeshName });
            AccessTools
                .DeclaredPropertySetter(typeof(ItemObject), "ItemHolsters")
                .Invoke(item, new object[] { itemToCopy.ItemHolsters });
            AccessTools
                .DeclaredPropertySetter(typeof(ItemObject), "FlyingMeshName")
                .Invoke(item, new object[] { itemToCopy.FlyingMeshName });
            AccessTools
                .DeclaredPropertySetter(typeof(ItemObject), "HolsterPositionShift")
                .Invoke(item, new object[] { itemToCopy.HolsterPositionShift });
            AccessTools
                .DeclaredPropertySetter(typeof(ItemObject), "BodyName")
                .Invoke(item, new object[] { itemToCopy.BodyName });
            AccessTools
                .DeclaredPropertySetter(typeof(ItemObject), "HolsterBodyName")
                .Invoke(item, new object[] { itemToCopy.HolsterBodyName });
            AccessTools
                .DeclaredPropertySetter(typeof(ItemObject), "CollisionBodyName")
                .Invoke(item, new object[] { itemToCopy.CollisionBodyName });
            AccessTools
                .DeclaredPropertySetter(typeof(ItemObject), "RecalculateBody")
                .Invoke(item, new object[] { itemToCopy.RecalculateBody });
            AccessTools
                .DeclaredPropertySetter(typeof(ItemObject), "PrefabName")
                .Invoke(item, new object[] { itemToCopy.PrefabName });
            AccessTools
                .DeclaredPropertySetter(typeof(ItemObject), "HolsterBodyName")
                .Invoke(item, new object[] { itemToCopy.HolsterBodyName });
            AccessTools
                .DeclaredPropertySetter(typeof(ItemObject), "ItemFlags")
                .Invoke(item, new object[] { itemToCopy.ItemFlags });
            AccessTools
                .DeclaredPropertySetter(typeof(ItemObject), "Value")
                .Invoke(item, new object[] { itemToCopy.Value });
            AccessTools
                .DeclaredPropertySetter(typeof(ItemObject), "Weight")
                .Invoke(item, new object[] { itemToCopy.Weight });
            AccessTools
                .DeclaredPropertySetter(typeof(ItemObject), "Difficulty")
                .Invoke(item, new object[] { itemToCopy.Difficulty });
            AccessTools
                .DeclaredPropertySetter(typeof(ItemObject), "ArmBandMeshName")
                .Invoke(item, new object[] { itemToCopy.ArmBandMeshName });
            AccessTools
                .DeclaredPropertySetter(typeof(ItemObject), "IsFood")
                .Invoke(item, new object[] { itemToCopy.IsFood });
            AccessTools
                .DeclaredPropertySetter(typeof(ItemObject), "ScaleFactor")
                .Invoke(item, new object[] { itemToCopy.ScaleFactor });
            AccessTools
                .DeclaredPropertySetter(typeof(ItemObject), "IsUniqueItem")
                .Invoke(item, new object[] { itemToCopy.IsUniqueItem });
            AccessTools
                .DeclaredPropertySetter(typeof(ItemObject), "WeaponDesign")
                .Invoke(item, new object[] { itemToCopy.WeaponDesign });
            AccessTools
                .DeclaredPropertySetter(typeof(ItemObject), "Culture")
                .Invoke(item, new object[] { itemToCopy.Culture });

            item.DetermineItemCategoryForItem();
            item.Type = itemToCopy.Type;
            item.IsReady = true;
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
