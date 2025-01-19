using System;
using EOAE_Code.Agents;
using EOAE_Code.Data.Xml;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic.Spells;

public class SummonSpell : Spell
{
    private const float SPAWNED_TIME_LEFT_TO_DISMISS = 3;

    public override bool IsThrown => false;

    private readonly SummonSpellData data;

    public SummonSpell(SpellDataXml data)
        : base(data)
    {
        if (data.SummonSpellData == null)
        {
            throw new Exception("Summon data is null");
        }

        this.data = data.SummonSpellData;
    }

    public override void Cast(Agent caster)
    {
        var summonerComponent = caster.GetComponent<SummonerAgentComponent>();
        if (summonerComponent == null)
        {
            summonerComponent = new SummonerAgentComponent(caster);
            caster.AddComponent(summonerComponent);
        }

        summonerComponent.Summon(caster, GetAimedPosition(caster), data);
    }

    public override bool IsAICastValid(Agent caster)
    {
        var summonerComponent = caster.GetComponent<SummonerAgentComponent>();
        if (summonerComponent == null)
        {
            return true;
        }

        if (summonerComponent.HasAnyActiveSummons() && Mission.Current.CurrentTime - summonerComponent.LastSummonTime > SPAWNED_TIME_LEFT_TO_DISMISS)
        {
            return false;
        }

        return true;
    }
}
