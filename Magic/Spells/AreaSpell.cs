using EOAE_Code.Data.Xml;
using EOAE_Code.Data.Xml.Spells;
using EOAE_Code.Interfaces;
using EOAE_Code.Magic.Spells.BombardTargeting;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic.Spells
{
    // Todo: this will probably include some visual effects like lightning etc
    public class AreaSpell : Spell, IUseAreaAim
    {
        public override bool IsThrown => false;
        private readonly AreaEffectData data;

        public float Range { get; private set; }
        public float Radius { get; private set; }
        public string AreaAimPrefab { get; private set; }

        public AreaSpell(SpellData data)
            : base(data)
        {
            AreaSpellData bombardSpellData = data as AreaSpellData;

            Range = bombardSpellData.Range;
            Radius = bombardSpellData.Radius;
            AreaAimPrefab = bombardSpellData.AreaAimPrefab;

            this.data = bombardSpellData.Effect;
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

            SpawnEffectHandler(playerCastFrame, caster);
        }

        public override bool IsAICastValid(Agent caster)
        {
            return GetTargeting().QuickValidate(caster, this);
        }

        private void SpawnEffectHandler(MatrixFrame castFrame, Agent caster)
        {
            var handlerEntity = GameEntity.Instantiate(Mission.Current.Scene, data.Prefab, false);
            handlerEntity.SetGlobalFrame(
                new MatrixFrame(
                    castFrame.rotation,
                    new Vec3(
                        castFrame.origin.X,
                        castFrame.origin.y,
                        castFrame.origin.z + data.Height
                    )
                )
            );
            handlerEntity.CreateAndAddScriptComponent(nameof(AreaSpellEffectHandler));

            var effectHandler = handlerEntity.GetFirstScriptOfType<AreaSpellEffectHandler>();
            effectHandler.Caster = caster;
            effectHandler.Data = data;
            effectHandler.Radius = Radius;

            effectHandler.SetScriptComponentToTick(ScriptComponentBehavior.TickRequirement.Tick);
        }
    }
}
