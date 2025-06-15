using EOAE_Code.Character;
using EOAE_Code.Data.Xml.Spells;
using EOAE_Code.Interfaces;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace EOAE_Code.Magic.Spells
{
    public abstract class Spell
    {
        public string Name { get; private set; }
        public string ItemName { get; private set; }
        public int Cost { get; private set; }
        public string SchoolName { get; private set; }
        public string Icon { get; private set; }
        public string Animation { get; private set; }
        public abstract bool IsThrown { get; }

        public SkillObject School
        {
            get
            {
                switch (SchoolName)
                {
                    case "Destruction":
                        return CustomSkills.Instance.Destruction;
                    case "Restoration":
                        return CustomSkills.Instance.Restoration;
                    case "Conjuration":
                        return CustomSkills.Instance.Conjuration;
                }

                return CustomSkills.Instance.Destruction;
            }
        }

        public Spell(SpellData data)
        {
            Name = new TextObject(data.Name).ToString();
            ItemName = data.ItemName;
            Cost = data.Cost;
            Icon = data.Icon;
            SchoolName = data.SchoolName;
            Animation = data.Animation;
        }

        public abstract void Cast(Agent caster);

        public abstract bool IsAICastValid(Agent caster);

        protected MatrixFrame GetAimedFrame(Agent caster)
        {
            if (this is IUseAreaAim && caster.IsPlayerControlled)
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
