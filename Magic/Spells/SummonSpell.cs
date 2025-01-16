using System;
using EOAE_Code.Agents;
using EOAE_Code.Data.Xml;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic.Spells;

public class SummonSpell : Spell
{
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
}
