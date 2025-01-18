using System;
using System.Collections.Generic;
using EOAE_Code.Data.Xml;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.TwoDimension;

namespace EOAE_Code.Magic.Spells;

public class BombardSpell : Spell
{
    public override bool IsThrown => false;
    private readonly BombardSpellData _data;

    public BombardSpell(SpellDataXml data)
        : base(data)
    {
        if (data.BombardSpellData == null)
        {
            throw new Exception("Bombard spell data is missing");
        }

        _data = data.BombardSpellData;
    }

    public override void Cast(Agent caster)
    {
        // TODO: What's a good way to determine cast position for AI?
        var playerCastFrame = GetAimedFrame(caster);

        var missileSpawner = CreateMissileSpawner(playerCastFrame, caster);

        GenerateOffsetsWithinCircle(_data.MissileCount, AreaRange, _data.MinHeight, _data.MaxHeight)
            .ForEach(pos =>
                missileSpawner.SpawnMissile(
                    _data.MissileName,
                    pos,
                    new Vec3(0.01f, 0.01f, -1),
                    _data.MissileSpeed
                )
            );

        missileSpawner.GameEntity.Remove(80);
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
