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
                "{=mbLBp63f}Apprentice Destruction",
                CustomSkills.Instance.Destruction,
                25,
                null,
                "{=xSw48XXU}Can learn apprentice spells",
                SkillEffect.PerkRole.Personal,
                0,
                SkillEffect.EffectIncrementType.Invalid
            );
            AdeptDestruction.Initialize(
                "{=gyabOx0V}Adept Destruction",
                CustomSkills.Instance.Destruction,
                100,
                null,
                "{=HE1SUCpQ}Can learn adept spells",
                SkillEffect.PerkRole.Personal,
                0,
                SkillEffect.EffectIncrementType.Invalid
            );
            ExpertDestruction.Initialize(
                "{=ykH7O1SM}Expert Destruction",
                CustomSkills.Instance.Destruction,
                175,
                null,
                "{=oiH5Nuyg}Can learn expert spells",
                SkillEffect.PerkRole.Personal,
                0,
                SkillEffect.EffectIncrementType.Invalid
            );
            MasterDestruction.Initialize(
                "{=dba7Epiv}Master Destruction",
                CustomSkills.Instance.Destruction,
                250,
                null,
                "{=jFT76UZ1}Can learn master spells",
                SkillEffect.PerkRole.Personal,
                0,
                SkillEffect.EffectIncrementType.Invalid
            );

            ApprenticeConjuration.Initialize(
                "{=LdfxEivW}Apprentice Conjuration",
                CustomSkills.Instance.Conjuration,
                25,
                null,
                "{=gQIVgPsS}Can learn apprentice spells",
                SkillEffect.PerkRole.Personal,
                0,
                SkillEffect.EffectIncrementType.Invalid
            );
            AdeptConjuration.Initialize(
                "{=zXkCQuUo}Adept Conjuration",
                CustomSkills.Instance.Conjuration,
                100,
                null,
                "{=92T2ds8C}Can learn adept spells",
                SkillEffect.PerkRole.Personal,
                0,
                SkillEffect.EffectIncrementType.Invalid
            );
            ExpertConjuration.Initialize(
                "{=Z0qjpU9J}Expert Conjuration",
                CustomSkills.Instance.Conjuration,
                175,
                null,
                "{=AmbRtiOh}Can learn expert spells",
                SkillEffect.PerkRole.Personal,
                0,
                SkillEffect.EffectIncrementType.Invalid
            );
            MasterConjuration.Initialize(
                "{=7gEDv0CF}Master Conjuration",
                CustomSkills.Instance.Conjuration,
                250,
                null,
                "{=wSI1xsQl}Can learn master spells",
                SkillEffect.PerkRole.Personal,
                0,
                SkillEffect.EffectIncrementType.Invalid
            );

            ApprenticeRestoration.Initialize(
                "{=vDTOOCD3}Apprentice Restoration",
                CustomSkills.Instance.Restoration,
                25,
                null,
                "{=VRbtelpk}Can learn apprentice spells",
                SkillEffect.PerkRole.Personal,
                0,
                SkillEffect.EffectIncrementType.Invalid
            );
            AdeptRestoration.Initialize(
                "{=VXCQathC}Adept Restoration",
                CustomSkills.Instance.Restoration,
                100,
                null,
                "{=6NhHrkLF}Can learn adept spells",
                SkillEffect.PerkRole.Personal,
                0,
                SkillEffect.EffectIncrementType.Invalid
            );
            ExpertRestoration.Initialize(
                "{=kSztGC4X}Expert Restoration",
                CustomSkills.Instance.Restoration,
                175,
                null,
                "{=NuagJ5IG}Can learn expert spells",
                SkillEffect.PerkRole.Personal,
                0,
                SkillEffect.EffectIncrementType.Invalid
            );
            MasterRestoration.Initialize(
                "{=EDkjmSOY}Master Restoration",
                CustomSkills.Instance.Restoration,
                250,
                null,
                "{=6XeZ0kxn}Can learn master spells",
                SkillEffect.PerkRole.Personal,
                0,
                SkillEffect.EffectIncrementType.Invalid
            );
        }
    }
}
