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
        private const float SPELLCASTING_DELAY = 3.5f;
        private float timeSinceLastCast = 0;

        private EquipmentIndex equipmentIndex = EquipmentIndex.ExtraWeaponSlot;
        private bool isSlotSetupDone = false;

        private TroopSpellBookData spellBook;

        public AICastingComponent(Agent agent)
            : base(agent)
        {
            if (!agent.IsHero)
            {
                spellBook = TroopSpellBookManager.GetSpellBookForTroop(agent.Character.StringId);
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
            if (
                spell != null
                && MagicMissionLogic.GetAgentCurrentMana(Agent) >= spell.Cost
                && spell.IsAICastValid(Agent)
            )
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
                        equipmentIndex,
                        Agent.WeaponWieldActionType.Instant,
                        false
                    );

                    if (!spell.IsThrown)
                    {
                        Agent.SetActionChannel(1, AnimationDurationManager.GetCacheIndex(spell));

                        MagicMissionLogic.AgentAnimationTimers.Add(
                            new AgentAnimationTimer(
                                AnimationDurationManager.GetDuration(spell),
                                () =>
                                {
                                    if (Agent.IsActive())
                                    {
                                        spell.Cast(Agent);
                                        Agent.SetWeaponAmountInSlot(equipmentIndex, 0, false);
                                    }
                                }
                            )
                        );
                    }
                }
            }
        }
    }
}
