using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace EOAE_Code.Character
{
    public class CustomSkillEffects
    {
        private static CustomSkillEffects? instance;

        public static CustomSkillEffects Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CustomSkillEffects();
                }

                return instance;
            }
        }

        public SkillEffect DestructionDamage { get; private set; }
        public SkillEffect RestorationHeal { get; private set; }
        public SkillEffect ConjurationDuration { get; private set; }

        public void Initialize()
        {
            DestructionDamage = Game.Current.ObjectManager.RegisterPresumedObject(
                new SkillEffect("DestructionDamage")
            );
            RestorationHeal = Game.Current.ObjectManager.RegisterPresumedObject(
                new SkillEffect("RestorationHeal")
            );
            ConjurationDuration = Game.Current.ObjectManager.RegisterPresumedObject(
                new SkillEffect("ConjurationDuration")
            );

            DestructionDamage.Initialize(
                new TextObject("{=eaPW4J70}Destruction spells damage: +{a0} %"),
                new SkillObject[] { CustomSkills.Instance.Destruction },
                SkillEffect.PerkRole.Personal,
                0.1f,
                SkillEffect.PerkRole.None,
                0,
                SkillEffect.EffectIncrementType.AddFactor,
                0,
                0
            );

            RestorationHeal.Initialize(
                new TextObject("{=SaRDNkCn}Restoration spells heal: +{a0} %"),
                new SkillObject[] { CustomSkills.Instance.Restoration },
                SkillEffect.PerkRole.Personal,
                0.2f,
                SkillEffect.PerkRole.None,
                0,
                SkillEffect.EffectIncrementType.AddFactor,
                0,
                0
            );

            ConjurationDuration.Initialize(
                new TextObject("{=WJCOjwML}Conjuration spells duration: +{a0} %"),
                new SkillObject[] { CustomSkills.Instance.Conjuration },
                SkillEffect.PerkRole.Personal,
                0.2f,
                SkillEffect.PerkRole.None,
                0,
                SkillEffect.EffectIncrementType.AddFactor,
                0,
                0
            );
        }
    }
}
