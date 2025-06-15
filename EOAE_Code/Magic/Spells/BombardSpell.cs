using System.Collections.Generic;
using EOAE_Code.Data.Xml;
using EOAE_Code.Data.Xml.Spells;
using EOAE_Code.Interfaces;
using EOAE_Code.Magic.Spells.BombardTargeting;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.TwoDimension;

namespace EOAE_Code.Magic.Spells;

public class BombardSpell : Spell, IUseAreaAim
{
    public override bool IsThrown => false;
    private readonly BombardMissileData _data;

    public float Range { get; private set; }
    public float Radius { get; private set; }
    public string AreaAimPrefab { get; private set; }

    public BombardSpell(SpellData data)
        : base(data)
    {
        BombardSpellData bombardSpellData = data as BombardSpellData;

        Range = bombardSpellData.Range;
        Radius = bombardSpellData.Radius;
        AreaAimPrefab = bombardSpellData.AreaAimPrefab;

        _data = bombardSpellData.BombardMissile;
    }

    private BombardTargetingBase GetTargeting()
    {
        // With lot of agents sampling might be faster
        if (Mission.Current.Agents.Count > 1000)
        {
            return BombardSamplingTargeting.Instance;
        }

        return BombardAgentTargeting.Instance;
    }

    public override void Cast(Agent caster)
    {
        var playerCastFrame = caster.IsPlayerControlled
            ? GetAimedFrame(caster)
            : GetTargeting().GetBestFrame(caster, this);
        if (playerCastFrame == MatrixFrame.Zero)
        {
            return;
        }

        var missileSpawner = CreateMissileSpawner(playerCastFrame, caster);

        GenerateOffsetsWithinCircle(_data.MissileCount, Radius, _data.MinHeight, _data.MaxHeight)
            .ForEach(pos =>
                missileSpawner.SpawnMissile(
                    _data.MissileName,
                    pos,
                    new Vec3(0.01f, 0.01f, -1),
                    _data.MissileSpeed,
                    _data.Effect
                )
            );

        missileSpawner.GameEntity.Remove(80);
    }

    public override bool IsAICastValid(Agent caster)
    {
        return GetTargeting().QuickValidate(caster, this);
    }

    private static MissileSpawner CreateMissileSpawner(MatrixFrame castFrame, Agent caster)
    {
        var spawnerEntity = GameEntity.Instantiate(Mission.Current.Scene, "__empty_object", false);
        spawnerEntity.SetGlobalFrame(castFrame);
        spawnerEntity.CreateAndAddScriptComponent(nameof(MissileSpawner));

        var missileSpawner = spawnerEntity.GetFirstScriptOfType<MissileSpawner>();
        missileSpawner.Caster = caster;

        return missileSpawner;
    }

    private static List<Vec3> GenerateOffsetsWithinCircle(
        int count,
        float radius,
        float minHeight,
        float maxHeight
    )
    {
        var offsets = new List<Vec3>();

        for (var i = 0; i < count; i++)
        {
            var angle = MBRandom.RandomFloat * Mathf.PI * 2;
            var distance = MBRandom.RandomFloat * radius;

            offsets.Add(
                new Vec3(
                    Mathf.Cos(angle) * distance,
                    Mathf.Sin(angle) * distance,
                    minHeight + MBRandom.RandomFloat * (maxHeight - minHeight)
                )
            );
        }

        return offsets;
    }
}
