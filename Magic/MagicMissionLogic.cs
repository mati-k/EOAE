using System;
using System.Collections.Generic;
using EOAE_Code.AI;
using EOAE_Code.Data.Managers;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic
{
    public class MagicMissionLogic : MissionLogic
    {
        public static Dictionary<Agent, float> CurrentMana = new();

        public override void AfterStart()
        {
            base.AfterStart();
            CurrentMana.Clear();
        }

        public override void OnAgentBuild(Agent agent, Banner banner)
        {
            base.OnAgentBuild(agent, banner);

            // No spellcasting horses for now
            if (!agent.IsHuman)
            {
                return;
            }

            if (agent.IsMainAgent)
            {
                CurrentMana.Add(agent, 100);
                return;
            }

            if (!agent.IsHero && TroopSpellBookManager.GetSpellBooxForTroop(agent.Character.StringId) != null)
            {
                agent.AddComponent(new AICastingComponent(agent));
                CurrentMana.Add(agent, 100);
            }
        }

        public override void OnAgentDeleted(Agent affectedAgent)
        {
            base.OnAgentDeleted(affectedAgent);

            if (CurrentMana.ContainsKey(affectedAgent))
            {
                CurrentMana.Remove(affectedAgent);
            }
        }

        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);

            foreach (Agent agent in this.Mission.Agents)
            {
                if (CurrentMana.ContainsKey(agent) && CurrentMana[agent] != 100f)
                {
                    float nextMana = Math.Min(CurrentMana[agent] + dt, 100f);
                    CurrentMana[agent] = nextMana;

                    if (!agent.IsPlayerControlled)
                    {
                        int manaCost = 0;
                        for (int i = 0; i < (int)EquipmentIndex.NumAllWeaponSlots; i++)
                        {
                            if (agent.Equipment[i].Item == null)
                            {
                                continue;
                            }

                            string itemId = agent.Equipment[i].Item.StringId;
                            if (SpellManager.IsSpell(itemId))
                            {
                                manaCost = SpellManager.GetSpellFromItem(itemId).Cost;
                                break;
                            }
                        }

                        if (
                            nextMana >= manaCost
                            && agent.Formation?.FiringOrder == FiringOrder.FiringOrderFireAtWill
                        )
                        {
                            agent.SetFiringOrder(FiringOrder.RangedWeaponUsageOrderEnum.FireAtWill);
                        }
                    }
                }
            }
        }
    }
}
