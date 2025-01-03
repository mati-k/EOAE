using EOAE_Code.Data.Managers;
using EOAE_Code.Magic;
using EOAE_Code.Magic.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace EOAE_Code.AI
{
    public class AICastingComponent : HumanAIComponent
    {
        private const float SPELLCASTING_DELAY = 2.5f;
        private float timeSinceLastCast = 0;

        private Spell spell;

        public AICastingComponent(Agent agent) : base(agent)
        {
            // ToDo: proper setup of spells and stuff
            spell = SpellManager.GetSpellFromItem("fireball");
        }

        public override void OnTickAsAI(float dt)
        {
            timeSinceLastCast += dt;
            if (timeSinceLastCast > SPELLCASTING_DELAY)
            {
                timeSinceLastCast = 0;
                RunSpellcastingLogic();
            }
        }

        public void RunSpellcastingLogic()
        {
            if (Agent.Equipment == null)
            {
                return;
            }

            // If is already using spell, don't interrupt
            if (Agent.GetWieldedItemIndex(Agent.HandIndex.MainHand) == EquipmentIndex.ExtraWeaponSlot)
            {
                return;
            }

            if (MagicMissionLogic.CurrentMana[Agent] >= spell.Cost)
            {
                ItemObject spellObject = MBObjectManager.Instance.GetObject<ItemObject>(spell.ItemName);
                MissionWeapon spawnedSpell = new MissionWeapon(spellObject, null, null, 1);
                
                Agent.EquipWeaponWithNewEntity(EquipmentIndex.ExtraWeaponSlot, ref spawnedSpell);
                Agent.TryToWieldWeaponInSlot(EquipmentIndex.ExtraWeaponSlot, Agent.WeaponWieldActionType.Instant, false);
            }
        }
    }
}
