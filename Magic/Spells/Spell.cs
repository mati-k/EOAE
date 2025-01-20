using EOAE_Code.Data.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic.Spells
{
    public abstract class Spell
    {
        public string Name { get; private set; }
        public string ItemName { get; private set; }
        public int Cost { get; private set; }
        public float EffectValue { get; private set; }
        public float Range { get; private set; }
        public float AreaRange { get; private set; }
        public SkillObject School { get; private set; }
        public bool AreaAim { get; private set; }
        public string AreaAimPrefab { get; private set; }
        public string Icon { get; private set; }
        public string Animation { get; private set; }
        public abstract bool IsThrown { get; }

        public Spell(SpellDataXml data)
        {
            Name = data.Name;
            ItemName = data.ItemName;
            Cost = data.Cost;
            EffectValue = data.EffecValue;
            Range = data.Range;
            AreaRange = data.AreaRange;
            AreaAim = data.AreaAim;
            AreaAimPrefab = data.AreaAimPrefab;
            Icon = data.Icon;
            School = data.School;
            Animation = data.Animation;
        }

        public abstract void Cast(Agent caster);

        public abstract bool IsAICastValid(Agent caster);

        protected MatrixFrame GetAimedFrame(Agent caster)
        {
            if (AreaAim && caster.IsPlayerControlled)
            {
                return Mission.Current.GetMissionBehavior<SpellAimView>().LastAimFrame;
            }

            return caster.GetWorldFrame().ToGroundMatrixFrame().Advance(3);
        }

        protected Vec3 GetAimedPosition(Agent caster)
        {
            return GetAimedFrame(caster).origin;
        }
    }
}
