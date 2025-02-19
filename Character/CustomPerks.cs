using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.Core;

namespace EOAE_Code.Character
{
    public class CustomPerks
    {
        private static CustomPerks? instance;

        public static CustomPerks Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CustomPerks();
                }

                return instance;
            }
        }

        public PerkObject ApprenticeDestruction { get; private set; }
        public PerkObject AdeptDestruction { get; private set; }
        public PerkObject ExpertDestruction { get; private set; }
        public PerkObject MasterDestruction { get; private set; }

        public PerkObject ApprenticeConjuration { get; private set; }
        public PerkObject AdeptConjuration { get; private set; }
        public PerkObject ExpertConjuration { get; private set; }
        public PerkObject MasterConjuration { get; private set; }

        public PerkObject ApprenticeRestoration { get; private set; }
        public PerkObject AdeptRestoration { get; private set; }
        public PerkObject ExpertRestoration { get; private set; }
        public PerkObject MasterRestoration { get; private set; }

        public void Initialize()
        {
            CreatePerks();
            InitializePerks();
        }

        private void CreatePerks()
        {
            ApprenticeDestruction = Game.Current.ObjectManager.RegisterPresumedObject(
                new PerkObject("ApprenticeDestruction")
            );
            AdeptDestruction = Game.Current.ObjectManager.RegisterPresumedObject(
                new PerkObject("AdeptDestruction")
            );
            ExpertDestruction = Game.Current.ObjectManager.RegisterPresumedObject(
                new PerkObject("ExpertDestruction")
            );
            MasterDestruction = Game.Current.ObjectManager.RegisterPresumedObject(
                new PerkObject("MasterDestruction")
            );

            ApprenticeConjuration = Game.Current.ObjectManager.RegisterPresumedObject(
                new PerkObject("ApprenticeConjuration")
            );
            AdeptConjuration = Game.Current.ObjectManager.RegisterPresumedObject(
                new PerkObject("AdeptConjuration")
            );
            ExpertConjuration = Game.Current.ObjectManager.RegisterPresumedObject(
                new PerkObject("ExpertConjuration")
            );
            MasterConjuration = Game.Current.ObjectManager.RegisterPresumedObject(
                new PerkObject("MasterConjuration")
            );

            ApprenticeRestoration = Game.Current.ObjectManager.RegisterPresumedObject(
                new PerkObject("ApprenticeRestoration")
            );
            AdeptRestoration = Game.Current.ObjectManager.RegisterPresumedObject(
                new PerkObject("AdeptRestoration")
            );
            ExpertRestoration = Game.Current.ObjectManager.RegisterPresumedObject(
                new PerkObject("ExpertRestoration")
            );
            MasterRestoration = Game.Current.ObjectManager.RegisterPresumedObject(
                new PerkObject("MasterRestoration")
            );
        }

        private void InitializePerks()
        {
            ApprenticeDestruction.Initialize(
                "Apprentice Destruction",
                CustomSkills.Instance.Destruction,
                25,
                null,
                "Can learn apprentice spells",
                SkillEffect.PerkRole.Personal,
                0,
                SkillEffect.EffectIncrementType.Invalid
            );
            AdeptDestruction.Initialize(
                "Adept Destruction",
                CustomSkills.Instance.Destruction,
                100,
                null,
                "Can learn adept spells",
                SkillEffect.PerkRole.Personal,
                0,
                SkillEffect.EffectIncrementType.Invalid
            );
            ExpertDestruction.Initialize(
                "Expert Destruction",
                CustomSkills.Instance.Destruction,
                175,
                null,
                "Can learn expert spells",
                SkillEffect.PerkRole.Personal,
                0,
                SkillEffect.EffectIncrementType.Invalid
            );
            MasterDestruction.Initialize(
                "Master Destruction",
                CustomSkills.Instance.Destruction,
                250,
                null,
                "Can learn master spells",
                SkillEffect.PerkRole.Personal,
                0,
                SkillEffect.EffectIncrementType.Invalid
            );

            ApprenticeConjuration.Initialize(
                "Apprentice Conjuration",
                CustomSkills.Instance.Conjuration,
                25,
                null,
                "Can learn apprentice spells",
                SkillEffect.PerkRole.Personal,
                0,
                SkillEffect.EffectIncrementType.Invalid
            );
            AdeptConjuration.Initialize(
                "Adept Conjuration",
                CustomSkills.Instance.Conjuration,
                100,
                null,
                "Can learn adept spells",
                SkillEffect.PerkRole.Personal,
                0,
                SkillEffect.EffectIncrementType.Invalid
            );
            ExpertConjuration.Initialize(
                "Expert Conjuration",
                CustomSkills.Instance.Conjuration,
                175,
                null,
                "Can learn expert spells",
                SkillEffect.PerkRole.Personal,
                0,
                SkillEffect.EffectIncrementType.Invalid
            );
            MasterConjuration.Initialize(
                "Master Conjuration",
                CustomSkills.Instance.Conjuration,
                250,
                null,
                "Can learn master spells",
                SkillEffect.PerkRole.Personal,
                0,
                SkillEffect.EffectIncrementType.Invalid
            );

            ApprenticeRestoration.Initialize(
                "Apprentice Restoration",
                CustomSkills.Instance.Restoration,
                25,
                null,
                "Can learn apprentice spells",
                SkillEffect.PerkRole.Personal,
                0,
                SkillEffect.EffectIncrementType.Invalid
            );
            AdeptRestoration.Initialize(
                "Adept Restoration",
                CustomSkills.Instance.Restoration,
                100,
                null,
                "Can learn adept spells",
                SkillEffect.PerkRole.Personal,
                0,
                SkillEffect.EffectIncrementType.Invalid
            );
            ExpertRestoration.Initialize(
                "Expert Restoration",
                CustomSkills.Instance.Restoration,
                175,
                null,
                "Can learn expert spells",
                SkillEffect.PerkRole.Personal,
                0,
                SkillEffect.EffectIncrementType.Invalid
            );
            MasterRestoration.Initialize(
                "Master Restoration",
                CustomSkills.Instance.Restoration,
                250,
                null,
                "Can learn master spells",
                SkillEffect.PerkRole.Personal,
                0,
                SkillEffect.EffectIncrementType.Invalid
            );
        }
    }
}
