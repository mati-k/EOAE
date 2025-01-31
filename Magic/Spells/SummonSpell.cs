using System;
using System.Collections.Generic;
using EOAE_Code.Agents;
using EOAE_Code.Data.Xml;
using EOAE_Code.Data.Xml.Spells;
using EOAE_Code.Interfaces;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic.Spells;

public class SummonSpell : Spell, IUseAreaAim
{
    private const float SPAWNED_TIME_LEFT_TO_DISMISS = 3;

    public override bool IsThrown => false;

    public float Range { get; private set; }
    public float Radius { get; private set; }
    public List<SummonEntityData> SummonEntities { get; private set; }
    public string AreaAimPrefab { get; private set; }

    public SummonSpell(SpellData data)
        : base(data)
    {
        SummonSpellData summonSpellData = data as SummonSpellData;

        if (summonSpellData.SummonEntities.Count == 0)
        {
            throw new Exception("Summon spell must have at least one entity to summon");
        }

        Radius = summonSpellData.Radius;
        Radius = summonSpellData.Radius;
        SummonEntities = summonSpellData.SummonEntities;
        AreaAimPrefab = summonSpellData.AreaAimPrefab;
    }

    public override void Cast(Agent caster)
    {
        // Can not summon agents from AI tick, will crash the game (modifying agents while iterating over them)
        var summonerComponent = caster.GetComponent<SummonerAgentComponent>();
        if (summonerComponent == null)
        {
            summonerComponent = new SummonerAgentComponent(caster);
            caster.AddComponent(summonerComponent);
        }

        foreach (var data in SummonEntities)
        {
            summonerComponent.Summon(caster, GetAimedPosition(caster), data);
        }
    }

    public override bool IsAICastValid(Agent caster)
    {
        var summonerComponent = caster.GetComponent<SummonerAgentComponent>();
        if (summonerComponent == null)
        {
            return true;
        }

        if (summonerComponent.HasAnyActiveSummons() && summonerComponent.SummonDespawnTime - Mission.Current.CurrentTime > SPAWNED_TIME_LEFT_TO_DISMISS)
        {
            return false;
        }

        return true;
    }
}
