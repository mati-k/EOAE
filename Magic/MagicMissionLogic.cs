using System.Collections.Generic;
using EOAE_Code.AI;
using EOAE_Code.Data.Managers;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic
{
    public class MagicMissionLogic : MissionLogic
    {
        public static Dictionary<Agent, AgentMana> AgentsMana = new();
        public static List<AgentAnimationTimer> AgentAnimationTimers = new();

        public override void AfterStart()
        {
            base.AfterStart();
            AgentsMana.Clear();
            AgentAnimationTimers.Clear();
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
                AgentsMana.Add(agent, new AgentMana(agent));
                return;
            }

            if (
                agent.IsHero
                || TroopSpellBookManager.GetSpellBookForTroop(agent.Character.StringId) != null
            )
            {
                agent.AddComponent(new AICastingComponent(agent));
                AgentsMana.Add(agent, new AgentMana(agent));
            }
        }

        public override void OnAgentDeleted(Agent affectedAgent)
        {
            base.OnAgentDeleted(affectedAgent);

            if (AgentsMana.ContainsKey(affectedAgent))
            {
                AgentsMana.Remove(affectedAgent);
            }
        }

        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);
            TickAgentAnimationTimers(dt);

            foreach (Agent agent in this.Mission.Agents)
            {
                if (AgentsMana.ContainsKey(agent))
                {
                    AgentsMana[agent].ManaRegenTick(dt);
                }
            }
        }

        private void TickAgentAnimationTimers(float dt)
        {
            for (int i = AgentAnimationTimers.Count - 1; i >= 0; i--)
            {
                var agentAnimationTimer = AgentAnimationTimers[i];
                agentAnimationTimer.DurationLeft -= dt;

                if (agentAnimationTimer.DurationLeft <= 0)
                {
                    agentAnimationTimer.OnComplete();
                    AgentAnimationTimers.RemoveAt(i);
                }
            }
        }

        public static AgentMana? GetAgentMana(Agent agent)
        {
            if (AgentsMana.ContainsKey(agent))
            {
                return AgentsMana[agent];
            }

            return null;
        }

        public static float GetAgentCurrentMana(Agent agent)
        {
            if (AgentsMana.ContainsKey(agent))
            {
                return AgentsMana[agent].CurrentMana;
            }

            return 0;
        }

        public static float GetAgentMaxMana(Agent agent)
        {
            if (AgentsMana.ContainsKey(agent))
            {
                return AgentsMana[agent].MaxMana;
            }

            return 0;
        }
    }
}
