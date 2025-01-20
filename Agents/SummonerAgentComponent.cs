using System.Collections.Generic;
using System.Linq;
using EOAE_Code.Data.Xml;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.AgentOrigins;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace EOAE_Code.Agents;

public class SummonerAgentComponent : AgentComponent
{
    private readonly List<Agent> summonedAgents = new();
    public float SummonDespawnTime { get; private set; }

    public SummonerAgentComponent(Agent agent)
        : base(agent) { }

    public void Summon(Agent caster, Vec3 position, SummonSpellData data)
    {
        ClearSummons();

        for (var i = 0; i < data.Amount; i++)
        {
            SummonAgent(caster, position, data);
        }

        SummonDespawnTime = Mission.Current.CurrentTime + data.Duration;
    }

    private void SummonAgent(Agent caster, Vec3 position, SummonSpellData data)
    {
        var character = MBObjectManager.Instance.GetObject<CharacterObject>(data.AgentName);
        var agent = Mission.Current.SpawnTroop(
            new SimpleAgentOrigin(character),
            caster.Team != Mission.Current.PlayerEnemyTeam,
            true,
            false,
            false,
            0,
            0,
            true,
            true,
            true,
            position,
            caster.GetMovementDirection()
        );

        agent.AddComponent(new SummonedAgentComponent(agent, caster, data.Duration));
        agent.SetAgentFlags(
            (agent.GetAgentFlags() | AgentFlag.CanGetAlarmed) & ~AgentFlag.CanRetreat
        );
        agent.SetTeam(caster.Team, true);
        summonedAgents.Add(agent);
    }

    private void ClearSummons()
    {
        foreach (var summonedAgent in summonedAgents)
        {
            if (summonedAgent.State == AgentState.Active)
            {
                var summonedAgentComponent = summonedAgent.GetComponent<SummonedAgentComponent>();
                summonedAgentComponent.Die();
            }
        }

        summonedAgents.Clear();
    }

    public bool HasAnyActiveSummons()
    {
        return summonedAgents.Any(agent => agent.State == AgentState.Active);
    }
}
