using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;

namespace EOAE_Code.States.Spellbook
{
    [GameStateScreen(typeof(SpellbookState))]
    public class SpellbookScreen : ScreenBase, IGameStateListener
    {
        private GauntletLayer gauntletLayer;
        private SpellbookState state;
        private SpellbookVM vm;

        public SpellbookScreen(SpellbookState state)
        {
            this.state = state;
            state.RegisterListener(this);
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
                vm.CharacterSwitcher.HandleHotKeyNavigation(gauntletLayer);
            }
        }

        void IGameStateListener.OnActivate()
        {
            base.OnActivate();

            vm = new SpellbookVM();

            this.vm.SetDoneInputKey(
                HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit")
            );

            this.vm.CharacterSwitcher.RegisterHotKeys();

            gauntletLayer = new GauntletLayer(1, "GauntletLayer", true);
            gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
            this.gauntletLayer.Input.RegisterHotKeyCategory(
                HotKeyManager.GetCategory("GenericPanelGameKeyCategory")
            );
            this.gauntletLayer.Input.RegisterHotKeyCategory(
                HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory")
            );
            gauntletLayer.LoadMovie("Spellbook", vm);
            AddLayer(gauntletLayer);
            gauntletLayer.IsFocusLayer = true;
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
        }

        void IGameStateListener.OnInitialize()
        {
            base.OnInitialize();
        }

        private void Close()
        {
            Game.Current.GameStateManager.PopState();
        }
    }
}
