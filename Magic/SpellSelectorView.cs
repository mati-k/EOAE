using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.ViewModelCollection;

namespace EOAE_Code.Magic;

[DefaultView]
public class SpellSelectorView : MissionView
{
    private const int TimeSpeedRequestId = 8923;
    private SpellSelectorVM spellSelector;

    private bool HoldHandled
    {
        get => holdHandled;
        set
        {
            holdHandled = value;
            var missionScreen = MissionScreen;
            missionScreen?.SetRadialMenuActiveState(value);
        }
    }

    private bool IsDisplayingADialog
    {
        get
        {
            IMissionScreen missionScreenAsInterface = MissionScreen;
            return (missionScreenAsInterface != null && missionScreenAsInterface.GetDisplayDialog())
                || MissionScreen.IsRadialMenuActive
                || Mission.IsOrderMenuOpen;
        }
    }

    private float toggleHoldTime;
    private bool isSlowDownApplied;
    private bool holdHandled;
    private bool prevKeyDown;

    public override void OnBehaviorInitialize()
    {
        base.OnBehaviorInitialize();

        spellSelector = new SpellSelectorVM();
        var layer = new GauntletLayer(0);
        layer.LoadMovie("SpellSelector", spellSelector);
        MissionScreen.AddLayer(layer);
    }

    public override void OnMissionScreenTick(float dt)
    {
        if (
            MissionScreen.SceneLayer.Input.IsKeyDown(InputKey.F)
            && !IsDisplayingADialog
            && Agent.Main != null
            && Agent.Main.IsActive()
            && Mission.Mode != MissionMode.Deployment
            && Mission.Mode != MissionMode.CutScene
            && !MissionScreen.IsRadialMenuActive
        )
        {
            if (toggleHoldTime > 0.3f && !HoldHandled)
            {
                HandleOpeningHold();
                HoldHandled = true;
            }
            toggleHoldTime += dt;
            prevKeyDown = true;
        }
        else if (prevKeyDown && !MissionScreen.SceneLayer.Input.IsKeyDown(InputKey.F))
        {
            if (toggleHoldTime > 0.3f)
            {
                HandleClosingHold();
            }
            HoldHandled = false;
            toggleHoldTime = 0f;
            prevKeyDown = false;
        }
    }

    private void HandleOpeningHold()
    {
        spellSelector.OnToggle(true);
        MissionScreen.SetRadialMenuActiveState(true);
        if (!GameNetwork.IsMultiplayer && !isSlowDownApplied)
        {
            Mission.AddTimeSpeedRequest(new Mission.TimeSpeedRequest(0.25f, TimeSpeedRequestId));
            isSlowDownApplied = true;
        }
    }

    private void HandleClosingHold()
    {
        spellSelector.OnToggle(false);
        MissionScreen.SetRadialMenuActiveState(false);
        if (!GameNetwork.IsMultiplayer && isSlowDownApplied)
        {
            Mission.RemoveTimeSpeedRequest(TimeSpeedRequestId);
            isSlowDownApplied = false;
        }
    }
}
