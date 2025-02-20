using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace EOAE_Code.Character
{
    public class CustomSkills
    {
        private static CustomSkills? instance;

        public static CustomSkills Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CustomSkills();
                }

                return instance;
            }
        }

        public SkillObject Destruction { get; private set; }
        public SkillObject Restoration { get; private set; }
        public SkillObject Conjuration { get; private set; }

        public void Initialize()
        {
            Destruction = Game.Current.ObjectManager.RegisterPresumedObject(
                new SkillObject("Destruction")
            );
            Restoration = Game.Current.ObjectManager.RegisterPresumedObject(
                new SkillObject("Restoration")
            );
            Conjuration = Game.Current.ObjectManager.RegisterPresumedObject(
                new SkillObject("Conjuration")
            );
            Destruction
                .Initialize(
                    new TextObject("{=5o7F9ZFA}Destruction"),
                    new TextObject("{=P7SIPHVb}Destruction, break shit!"),
                    SkillObject.SkillTypeEnum.Personal
                )
                .SetAttribute(Attributes.Instance.Magic);
            Restoration
                .Initialize(
                    new TextObject("{=NCf2MEdC}Restoration"),
                    new TextObject("{=9a1GPLFY}Restoration, fix shit!"),
                    SkillObject.SkillTypeEnum.Personal
                )
                .SetAttribute(Attributes.Instance.Magic);
            Conjuration
                .Initialize(
                    new TextObject("{=5C8nzLIQ}Conjuration"),
                    new TextObject("{=XQ4epc7d}Conjuration, summon shit!"),
                    SkillObject.SkillTypeEnum.Personal
                )
                .SetAttribute(Attributes.Instance.Magic);
        }
    }
}
