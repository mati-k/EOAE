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
                CustomSkills.Instance.Destruction,
                PartyRole.Personal,
                0.1f,
                EffectIncrementType.AddFactor,
                0,
                0,
                0
            );

            RestorationHeal.Initialize(
                new TextObject("{=SaRDNkCn}Restoration spells heal: +{a0} %"),
                CustomSkills.Instance.Restoration,
                PartyRole.Personal,
                0.2f,
                EffectIncrementType.AddFactor,
                0,
                0,
                0
            );

            ConjurationDuration.Initialize(
                new TextObject("{=WJCOjwML}Conjuration spells duration: +{a0} %"),
                CustomSkills.Instance.Conjuration,
                PartyRole.Personal,
                0.2f,
                EffectIncrementType.AddFactor,
                0,
                0,
                0
            );
        }
    }
}
