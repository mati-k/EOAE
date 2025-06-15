using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace EOAE_Code.States.Enchantment
{
    [GameStateScreen(typeof(EnchantmentState))]
    public class EnchantmentScreen : ScreenBase, IGameStateListener
    {
        private GauntletLayer gauntletLayer;
        private EnchantmentState state;
        private EnchantmentVM vm;

        private static readonly string[] SPRITE_CATEGORY_NAMES = new string[]
        {
            "ui_inventory",
            "ui_crafting",
        };

        private List<SpriteCategory> LoadedSpriteCategories = new();

        public EnchantmentScreen(EnchantmentState state)
        {
            this.state = state;
            state.RegisterListener(this);
        }

        private void LoadInventorySprites()
        {
            var spriteData = UIResourceManager.SpriteData;
            var resourceContext = UIResourceManager.ResourceContext;
            var resourceDepot = UIResourceManager.UIResourceDepot;

            LoadedSpriteCategories.Clear();
            foreach (var spriteCategoryName in SPRITE_CATEGORY_NAMES)
            {
                var spriteCategory = spriteData.SpriteCategories[spriteCategoryName];
                spriteCategory.Load(resourceContext, resourceDepot);
                LoadedSpriteCategories.Add(spriteCategory);
            }
        }

        protected override void OnFrameTick(float dt)
        {
            base.OnFrameTick(dt);
            LoadingWindow.DisableGlobalLoadingWindow();

            if (gauntletLayer.Input.IsHotKeyDownAndReleased("Exit"))
            {
                vm.ExecuteClose();
            }
            else
            {
                //vm.CharacterSwitcher.HandleHotKeyNavigation(gauntletLayer);
            }
        }

        void IGameStateListener.OnActivate()
        {
            base.OnActivate();

            vm = new EnchantmentVM();

            this.vm.SetDoneInputKey(
                HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit")
            );

            //this.vm.CharacterSwitcher.RegisterHotKeys();

            gauntletLayer = new GauntletLayer(1, "GauntletLayer", true);
            gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
            this.gauntletLayer.Input.RegisterHotKeyCategory(
                HotKeyManager.GetCategory("GenericPanelGameKeyCategory")
            );
            this.gauntletLayer.Input.RegisterHotKeyCategory(
                HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory")
            );
            gauntletLayer.LoadMovie("Enchantment", vm);
            AddLayer(gauntletLayer);
            gauntletLayer.IsFocusLayer = true;

            var context = gauntletLayer.UIContext;

            ScreenManager.TrySetFocus(gauntletLayer);
        }

        void IGameStateListener.OnDeactivate()
        {
            base.OnDeactivate();
            RemoveLayer(gauntletLayer);
            gauntletLayer.IsFocusLayer = false;
            ScreenManager.TryLoseFocus(gauntletLayer);
        }

        void IGameStateListener.OnFinalize()
        {
            base.OnFinalize();

            gauntletLayer = null;
            vm = null;

            foreach (var spriteCategory in LoadedSpriteCategories)
            {
                spriteCategory.Unload();
            }
            LoadedSpriteCategories.Clear();
        }

        void IGameStateListener.OnInitialize()
        {
            base.OnInitialize();
            LoadInventorySprites();
        }
    }
}
