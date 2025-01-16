using EOAE_Code.Data.Managers;
using EOAE_Code.Magic.Spells;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.ObjectSystem;

namespace EOAE_Code.Magic
{
    [DefaultView]
    public class MagicMissionView : MissionView
    {
        public bool UsingAreaAim => areaAimEntity != null;
        public MatrixFrame LastAreaAimFrame { get; private set; }

        private MagicHudVM magicHUD;
        private GauntletLayer magicLayer;
        private GameEntity? areaAimEntity;
        private Spell? aimedAreaSpell;

        public override void OnBehaviorInitialize()
        {
            base.OnBehaviorInitialize();

            magicHUD = new MagicHudVM(this.Mission);
            magicLayer = new GauntletLayer(0);
            magicLayer.LoadMovie("MagicHUD", magicHUD);
            MissionScreen.AddLayer(magicLayer);
        }

        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);

            if (magicHUD != null)
            {
                if (Input.IsKeyPressed(TaleWorlds.InputSystem.InputKey.Q))
                {
                    MagicPlayerManager.SwitchPlayerSpell(-1);
                }

                if (Input.IsKeyPressed(TaleWorlds.InputSystem.InputKey.E))
                {
                    MagicPlayerManager.SwitchPlayerSpell(1);
                }

                if (Input.IsKeyPressed(TaleWorlds.InputSystem.InputKey.F))
                {
                    EquipCurrentSpell();
                }

                magicHUD.Tick();
            }

            AreaAimTick();
        }

        private void EquipCurrentSpell()
        {
            Agent player = Mission.MainAgent;

            Spell spell = SpellManager.GetSpell(MagicPlayerManager.GetPlayerSpellIndex());
            ItemObject spellObject = MBObjectManager.Instance.GetObject<ItemObject>(spell.ItemName);
            MissionWeapon spellWeapon = new MissionWeapon(spellObject, null, null);

            player.EquipWeaponWithNewEntity(EquipmentIndex.ExtraWeaponSlot, ref spellWeapon);
            player.TryToWieldWeaponInSlot(
                EquipmentIndex.ExtraWeaponSlot,
                Agent.WeaponWieldActionType.Instant,
                false
            );

            UpdateAreaAim(spell);
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
