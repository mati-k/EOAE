using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace EOAE_Code.Character;

public class CustomCharacterCreationContent : SandboxCharacterCreationContent
{
    protected override void OnInitialized(CharacterCreation characterCreation)
    {
        base.OnInitialized(characterCreation);

        AddOption(
            characterCreation,
            CharacterCreationMenu.Childhood,
            new TextObject("{=ylSw2pTT}your curiosity for the arcane."),
            new MBList<SkillObject> { CustomSkills.Instance.Conjuration },
            Attributes.Instance.Magic,
            FocusToAdd,
            SkillLevelToAdd,
            AttributeLevelToAdd,
            new TextObject(
                "{=WF0vZxnh}As a child, you were always fascinated by the arcane. You would spend hours watching the court mages at work, your eyes wide with wonder as they summoned fire and ice with a flick of their wrists. You would often sneak into the library to read their spellbooks, even though you couldn't understand a word of what was written."
            )
        );
        AddOption(
            characterCreation,
            CharacterCreationMenu.Education,
            new TextObject("{=VTlTiMBJ}studied at a mage’s guild."),
            new MBList<SkillObject>
            {
                CustomSkills.Instance.Destruction,
                CustomSkills.Instance.Restoration,
                CustomSkills.Instance.Conjuration,
            },
            Attributes.Instance.Magic,
            FocusToAdd,
            SkillLevelToAdd,
            AttributeLevelToAdd,
            new TextObject(
                "{=VG1uaMla}Accepted into a mage’s guild at a young age, you spent your days poring over scrolls and learning the intricacies of the arcane arts. Though your practical experience was limited, your theoretical knowledge was vast."
            )
        );
        AddOption(
            characterCreation,
            CharacterCreationMenu.Youth,
            new TextObject("{=8y02G4T8}aided in a great healing."),
            new MBList<SkillObject> { CustomSkills.Instance.Restoration },
            DefaultCharacterAttributes.Social,
            FocusToAdd,
            SkillLevelToAdd,
            AttributeLevelToAdd,
            new TextObject(
                "{=kiJgeq0w}A terrible plague swept through your home, and while others fled, you stayed to help. Whether through alchemy or magic, you saved lives, earning the gratitude of many."
            )
        );
        AddOption(
            characterCreation,
            CharacterCreationMenu.Adult,
            new TextObject("{=oQJlcPEa}you mastered an element."),
            new MBList<SkillObject> { CustomSkills.Instance.Destruction },
            Attributes.Instance.Magic,
            FocusToAdd,
            SkillLevelToAdd,
            AttributeLevelToAdd,
            new TextObject(
                "{=iPyMP3tN}Through rigorous study and practice, you became an expert in a single element—fire, frost, or lightning. Your spells in this domain are potent and feared."
            )
        );
    }

    private static void AddOption(
        CharacterCreation characterCreation,
        CharacterCreationMenu menu,
        TextObject text,
        MBList<SkillObject> skills,
        CharacterAttribute attribute,
        int focus,
        int skillLevel,
        int attributeLevel,
        TextObject description
    )
    {
        var characterCreationMenu = characterCreation.CharacterCreationMenus[(int)menu];
        foreach (var cultureCategory in characterCreationMenu.CharacterCreationCategories)
        {
            cultureCategory.AddCategoryOption(
                text,
                skills,
                attribute,
                focus,
                skillLevel,
                attributeLevel,
                null,
                null,
                Empty, // If this is null skills won't increase, so we need to pass at least an empty method
                description
            );
        }
    }

    private static void Empty(CharacterCreation characterCreation) { }

    private enum CharacterCreationMenu
    {
        Parents,
        Childhood,
        Education,
        Youth,
        Adult,
        Age
    }
}
