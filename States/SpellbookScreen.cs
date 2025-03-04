using EOAE_Code.Magic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.ScreenSystem;

namespace EOAE_Code.States
{
    [GameStateScreen(typeof(SpellbookState))]
    public class SpellbookScreen : ScreenBase, IGameStateListener
    {
        private const int SPELL_SLOTS = 5;

        private GauntletLayer gauntletLayer;
        private SpellbookState state;
        private MagicHudVM vm;

        public SpellbookScreen(SpellbookState state)
        {
            this.state = state;
            state.RegisterListener(this);
        }

        protected override void OnFrameTick(float dt)
        {
            base.OnFrameTick(dt);
            LoadingWindow.DisableGlobalLoadingWindow();
            if (
                gauntletLayer.Input.IsHotKeyDownAndReleased("Exit")
                || gauntletLayer.Input.IsGameKeyDownAndReleased(41)
            )
            {
                Close();
            }
        }

        void IGameStateListener.OnActivate()
        {
            base.OnActivate();

            vm = new MagicHudVM();
            gauntletLayer = new GauntletLayer(1, "GauntletLayer", true);
            gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
            gauntletLayer.Input.RegisterHotKeyCategory(
                HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory")
            );
            gauntletLayer.LoadMovie("MagicHUD", vm);
            gauntletLayer.IsFocusLayer = true;
            base.AddLayer(gauntletLayer);
            ScreenManager.TrySetFocus(gauntletLayer);
        }

        void IGameStateListener.OnDeactivate()
        {
            base.OnDeactivate();
        }

        void IGameStateListener.OnFinalize()
        {
            base.OnFinalize();
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
