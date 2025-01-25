using System.ComponentModel;
using EOAE_Code.Agents;
using EOAE_Code.Interfaces;
using EOAE_Code.Magic.Spells;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace EOAE_Code.Magic;

[DefaultView]
public class SpellAimView : MissionView
{
    public MatrixFrame LastAimFrame { get; private set; }

    private GameEntity? aimEntity;
    private Spell? equippedSpell;

    public override void OnBehaviorInitialize()
    {
        base.OnBehaviorInitialize();

        Mission.Current.OnMainAgentChanged += OnMainAgentChanged;
        OnMainAgentChanged(null, null);
    }

    public override void OnMissionTick(float dt)
    {
        base.OnMissionTick(dt);

        AimTick();
    }

    private void OnMainAgentChanged(object? sender, PropertyChangedEventArgs? e)
    {
        if (Agent.Main != null)
        {
            Agent.Main.OnAgentWieldedItemChange += OnMainAgentWieldedItemChange;
            OnMainAgentWieldedItemChange();
        }
    }

    private void OnMainAgentWieldedItemChange()
    {
        if (aimEntity != null)
        {
            aimEntity.Remove(80);
            aimEntity = null;
            equippedSpell = null;
        }

        var newSpell = Agent.Main?.GetEquippedSpell();
        if (newSpell is not IUseAreaAim useAreaAim)
            return;

        aimEntity = GameEntity.Instantiate(Mission.Scene, useAreaAim.AreaAimPrefab, false);
        aimEntity.SetMobility(GameEntity.Mobility.dynamic);
        equippedSpell = newSpell;
    }

    private void AimTick()
    {
        if (aimEntity == null || equippedSpell == null || Mission.MainAgent == null || equippedSpell is not IUseAreaAim areaAimable)
            return;

        var playerAgent = Mission.MainAgent;
        var playerFrame = playerAgent.GetWorldFrame().ToGroundMatrixFrame();
        var aimFrame = aimEntity.GetGlobalFrame();

        Mission.Scene.RayCastForClosestEntityOrTerrain(
            playerAgent.GetEyeGlobalPosition() + playerAgent.LookDirection,
            playerAgent.GetEyeGlobalPosition() + MissionScreen.CombatCamera.Direction * 3000f,
            out _,
            out var closestPoint,
            out _
        );

        var distance = MissionScreen.CombatCamera.Position.AsVec2 - closestPoint.AsVec2;
        if (areaAimable.Range > distance.Length)
        {
            closestPoint.z = MagicAgentUtils.GetHeightAtPoint(closestPoint.AsVec2, areaAimable);
            aimFrame.origin = closestPoint;
        }
        else
        {
            var playerLookDirection = playerFrame.rotation.f;
            playerLookDirection.z = 0;
            playerLookDirection.NormalizeWithoutChangingZ();

            // 3 is a magic number that fixes aim point snapping in third-person view
            var furthestPosition =
                playerFrame.origin + playerLookDirection * (areaAimable.Range - 3);
            furthestPosition.z = MagicAgentUtils.GetHeightAtPoint(furthestPosition.AsVec2, areaAimable);

            aimFrame.origin = furthestPosition;
        }

        var aimRotation = playerFrame.rotation;
        aimRotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
        aimFrame.rotation = aimRotation;
        aimFrame.Scale(Vec3.One * 0.0002f * areaAimable.Radius);

        aimEntity.SetGlobalFrame(aimFrame);
        LastAimFrame = aimFrame;
    }
}
