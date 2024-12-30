using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View;
using SandBox.BoardGames.AI;
using EOAE_Code.Data.Loaders;
using EOAE_Code.Data.Xml;
using TaleWorlds.CampaignSystem;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;
using EOAE_Code.Data.Managers;

namespace EOAE_Code.Magic
{
    [DefaultView]
    public class MagicMissionView : MissionView
    {
        private MagicHudVM magicHUD;
        private GauntletLayer magicLayer;

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
        }

        private void EquipCurrentSpell()
        {
            Agent player = Mission.MainAgent;

            SpellDataXml spell = SpellManager.GetSpell(MagicPlayerManager.GetPlayerSpellIndex());
            ItemObject spellObject = Game.Current.ObjectManager.GetObject<ItemObject>(spell.ItemName);
            MissionWeapon spellWeapon = new MissionWeapon(spellObject, null, null);

            player.EquipWeaponWithNewEntity(EquipmentIndex.ExtraWeaponSlot, ref spellWeapon);
            player.TryToWieldWeaponInSlot(EquipmentIndex.ExtraWeaponSlot, Agent.WeaponWieldActionType.Instant, false);
        }
    }
}
