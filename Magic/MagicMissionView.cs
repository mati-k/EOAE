using System;
using EOAE_Code.Data.Managers;
using EOAE_Code.Magic.Spells;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.ViewModelCollection;
using TaleWorlds.ObjectSystem;

namespace EOAE_Code.Magic
{
    [DefaultView]
    public class MagicMissionView : MissionView
    {
        public bool UsingAreaAim => areaAimEntity != null;
        public MatrixFrame LastAreaAimFrame { get; private set; }

        private MagicHudVM magicHUD;
        private MainAgentSpellControllerVM test;
        private GauntletLayer magicLayer;
        private GameEntity? areaAimEntity;
        private Spell? aimedAreaSpell;

        public override void OnBehaviorInitialize()
        {
            base.OnBehaviorInitialize();

            magicHUD = new MagicHudVM(Mission);
            try
            {
                test = new MainAgentSpellControllerVM();
                magicLayer = new GauntletLayer(0);
                magicLayer.LoadMovie("MainAgentSpellController", test);
                MissionScreen.AddLayer(magicLayer);
                test.Initialize();
            }
            catch (Exception e)
            {
                InformationManager.DisplayMessage(new InformationMessage(e.Message));
            }
        }

        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);

            if (magicHUD != null)
            {
                magicHUD.Tick();
            }

            AreaAimTick();
        }

        private bool holdHandled;

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
                return (
                        missionScreenAsInterface != null
                        && missionScreenAsInterface.GetDisplayDialog()
                    )
                    || MissionScreen.IsRadialMenuActive
                    || Mission.IsOrderMenuOpen;
            }
        }
        private float toggleHoldTime;
        private bool prevKeyDown;

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

        private bool isSlowDownApplied;

        private void HandleOpeningHold()
        {
            test.OnToggle(true);
            MissionScreen.SetRadialMenuActiveState(true);
            if (!GameNetwork.IsMultiplayer && !isSlowDownApplied)
            {
                Mission.AddTimeSpeedRequest(new Mission.TimeSpeedRequest(0.25f, 624));
                isSlowDownApplied = true;
            }
        }

        private void HandleClosingHold()
        {
            test.OnToggle(false);
            MissionScreen.SetRadialMenuActiveState(false);
            if (!GameNetwork.IsMultiplayer && isSlowDownApplied)
            {
                Mission.RemoveTimeSpeedRequest(624);
                isSlowDownApplied = false;
            }
        }

        private void UpdateAreaAim(Spell newSpell)
        {
            if (areaAimEntity != null)
            {
                areaAimEntity.Remove(80);
                areaAimEntity = null;
                aimedAreaSpell = null;
            }

            if (newSpell.AreaAim)
            {
                areaAimEntity = GameEntity.Instantiate(
                    Mission.Current.Scene,
                    newSpell.AreaAimPrefab,
                    false
                );
                areaAimEntity.SetMobility(GameEntity.Mobility.dynamic);
                aimedAreaSpell = newSpell;
            }
        }

        private void AreaAimTick()
        {
            if (areaAimEntity == null || aimedAreaSpell == null || Mission.MainAgent == null)
                return;

            var playerAgent = Mission.MainAgent;
            var playerFrame = playerAgent.GetWorldFrame().ToGroundMatrixFrame();
            var areaAimFrame = areaAimEntity.GetGlobalFrame();

            Mission.Scene.RayCastForClosestEntityOrTerrain(
                playerAgent.GetEyeGlobalPosition() + playerAgent.LookDirection,
                playerAgent.GetEyeGlobalPosition() + MissionScreen.CombatCamera.Direction * 3000f,
                out _,
                out var closestPoint,
                out _
            );

            var distance = MissionScreen.CombatCamera.Position.AsVec2 - closestPoint.AsVec2;

            if (distance.Length < aimedAreaSpell.Range)
            {
                closestPoint.z = GetHeightAtPoint(closestPoint.AsVec2);
                areaAimFrame.origin = closestPoint;
            }
            else
            {
                var playerLookDirection = playerFrame.rotation.f;
                playerLookDirection.z = 0;
                playerLookDirection.NormalizeWithoutChangingZ();

                var furthestPosition =
                    playerFrame.origin + playerLookDirection * (aimedAreaSpell.Range - 3);
                furthestPosition.z = GetHeightAtPoint(furthestPosition.AsVec2);

                areaAimFrame.origin = furthestPosition;
            }

            var areaAimRotation = playerFrame.rotation;
            areaAimRotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
            areaAimFrame.rotation = areaAimRotation;
            areaAimFrame.Scale(Vec3.One * 0.0002f * aimedAreaSpell.AreaRange);

            areaAimEntity.SetGlobalFrame(areaAimFrame);
            LastAreaAimFrame = areaAimFrame;
        }

        private float GetHeightAtPoint(Vec2 point)
        {
            float height = 0;
            Mission.Scene.GetHeightAtPoint(
                point,
                BodyFlags.CommonCollisionExcludeFlagsForCombat,
                ref height
            );

            // Bigger the area, higher the height for better visibility
            height += 0.1f * aimedAreaSpell.AreaRange;
            return height;
        }
    }
}
