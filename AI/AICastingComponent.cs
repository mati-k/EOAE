using EOAE_Code.Data.Managers;
using EOAE_Code.Data.Xml;
using EOAE_Code.Magic;
using EOAE_Code.Magic.Spells;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace EOAE_Code.AI
{
    public class AICastingComponent : HumanAIComponent
    {
        private const float SPELLCASTING_DELAY = 2.5f;
        private float timeSinceLastCast = 0;

        private EquipmentIndex equipmentIndex = EquipmentIndex.ExtraWeaponSlot;
        private bool isSlotSetupDone = false;

        private TroopSpellBookData spellBook;

        public AICastingComponent(Agent agent)
            : base(agent)
        {
            if (!agent.IsHero)
            {
                spellBook = TroopSpellBookManager.GetSpellBooxForTroop(agent.Character.StringId);
            }
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

        // Try to use unused slot for spells instead of extra one. Allows AI to keep spells / switch to them in regular way (mainly for basic thrower unit)
        private void SetupSpellSlot()
        {
            if (!isSlotSetupDone)
            {
                for (int slot = 0; slot < (int)EquipmentIndex.NumPrimaryWeaponSlots; slot++)
                {
                    if (this.Agent.Equipment[slot].Item == null)
                    {
                        equipmentIndex = (EquipmentIndex)slot;
                        return;
                    }
                }
            }
        }

        private void RunSpellcastingLogic()
        {
            if (Agent.Equipment == null)
            {
                return;
            }

            SetupSpellSlot();

            // If is already using spell, don't interrupt
            if (Agent.GetWieldedItemIndex(Agent.HandIndex.MainHand) == equipmentIndex)
            {
                return;
            }

            Spell spell = spellBook.GetRandomSpell();
            MagicMissionLogic.CurrentMana.TryGetValue(Agent, out var currentMana);
            if (spell != null && currentMana >= spell.Cost)
            {
                if (
                    Agent.Equipment[equipmentIndex].Item == null
                    || Agent.Equipment[equipmentIndex].Item.StringId != spell.ItemName
                )
                {
                    ItemObject spellObject = MBObjectManager.Instance.GetObject<ItemObject>(
                        spell.ItemName
                    );
                    MissionWeapon spawnedSpell = new MissionWeapon(spellObject, null, null, 1);

                    Agent.EquipWeaponWithNewEntity(equipmentIndex, ref spawnedSpell);
                }

                // If it's regular thrown spell and has free slot, there is no need to manually wield spell
                if (!spell.IsThrown || equipmentIndex == EquipmentIndex.ExtraWeaponSlot)
                {
                    Agent.TryToWieldWeaponInSlot(
                        EquipmentIndex.ExtraWeaponSlot,
                        Agent.WeaponWieldActionType.Instant,
                        false
                    );
                }
            }
        }
    }
}
