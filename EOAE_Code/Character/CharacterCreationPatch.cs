using HarmonyLib;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace EOAE_Code.Character;

/*
 * NarrativeMenuOption:
 * GetNarrativeMenuOptionArgsDelegate - used to set the skills, focus, and attribute levels granted by the option.
 * NarrativeMenuOptionOnConditionDelegate - used to determine if the option should be available.
 * NarrativeMenuOptionOnSelectDelegate - logic on select, animation, equip items, set title etc.
 * NarrativeMenuOptionOnConsequenceDelegate - only seems to be used for age selection
 */

[HarmonyPatch]
public class CharacterCreationPatch
{
    private const int FOCUS_TO_ADD = 1;
    private const int SKILL_LEVEL_TO_ADD = 10;
    private const int ATTRIBUTE_LEVEL_TO_ADD = 1;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CharacterCreationCampaignBehavior), "AddChildhoodNarrativeMenuOptions")]
    public static void AddCustomChildhoodOption(
        CharacterCreationCampaignBehavior __instance,
        ref NarrativeMenu narrativeMenu
    )
    {
        NarrativeMenuOption customOption = new NarrativeMenuOption(
            "childhood_arcane_option",
            new TextObject("{=ylSw2pTT}your curiosity for the arcane."),
            new TextObject(
                "{=WF0vZxnh}As a child, you were always fascinated by the arcane. You would spend hours watching the court mages at work, your eyes wide with wonder as they summoned fire and ice with a flick of their wrists. You would often sneak into the library to read their spellbooks, even though you couldn't understand a word of what was written."
            ),
            new GetNarrativeMenuOptionArgsDelegate(args =>
            {
                args.SetAffectedSkills(new SkillObject[] { CustomSkills.Instance.Conjuration });
                args.SetFocusToSkills(FOCUS_TO_ADD);
                args.SetLevelToSkills(SKILL_LEVEL_TO_ADD);
                args.SetLevelToAttribute(Attributes.Instance.Magic, SKILL_LEVEL_TO_ADD);
            }),
            new NarrativeMenuOptionOnConditionDelegate(args => true),
            null,
            null
        );
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CharacterCreationCampaignBehavior), "AddEducationMenuOptions")]
    public static void AddCustomEducationOption(
        CharacterCreationCampaignBehavior __instance,
        ref NarrativeMenu narrativeMenu
    )
    {
        NarrativeMenuOption customOption = new NarrativeMenuOption(
            "education_arcane_option",
            new TextObject("{=VTlTiMBJ}studied at a mage’s guild."),
            new TextObject(
                "{=VG1uaMla}Accepted into a mage’s guild at a young age, you spent your days poring over scrolls and learning the intricacies of the arcane arts. Though your practical experience was limited, your theoretical knowledge was vast."
            ),
            new GetNarrativeMenuOptionArgsDelegate(args =>
            {
                args.SetAffectedSkills(
                    new SkillObject[]
                    {
                        CustomSkills.Instance.Destruction,
                        CustomSkills.Instance.Restoration,
                        CustomSkills.Instance.Conjuration,
                    }
                );
                args.SetFocusToSkills(FOCUS_TO_ADD);
                args.SetLevelToSkills(SKILL_LEVEL_TO_ADD);
                args.SetLevelToAttribute(Attributes.Instance.Magic, SKILL_LEVEL_TO_ADD);
            }),
            new NarrativeMenuOptionOnConditionDelegate(args => true),
            null,
            null
        );
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CharacterCreationCampaignBehavior), "AddYouthMenuOptions")]
    public static void AddCustomYouthOption(
        CharacterCreationCampaignBehavior __instance,
        ref NarrativeMenu narrativeMenu
    )
    {
        NarrativeMenuOption customOption = new NarrativeMenuOption(
            "youth_arcane_option",
            new TextObject("{=8y02G4T8}aided in a great healing."),
            new TextObject(
                "{=kiJgeq0w}A terrible plague swept through your home, and while others fled, you stayed to help. Whether through alchemy or magic, you saved lives, earning the gratitude of many."
            ),
            new GetNarrativeMenuOptionArgsDelegate(args =>
            {
                args.SetAffectedSkills(new SkillObject[] { CustomSkills.Instance.Restoration });
                args.SetFocusToSkills(FOCUS_TO_ADD);
                args.SetLevelToSkills(SKILL_LEVEL_TO_ADD);
                args.SetLevelToAttribute(Attributes.Instance.Magic, SKILL_LEVEL_TO_ADD);
            }),
            new NarrativeMenuOptionOnConditionDelegate(args => true),
            null,
            null
        );
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(CharacterCreationCampaignBehavior), "AddAdulthoodMenuOptions")]
    public static void AddCustomAdulthoodOption(
        CharacterCreationCampaignBehavior __instance,
        ref NarrativeMenu narrativeMenu
    )
    {
        NarrativeMenuOption customOption = new NarrativeMenuOption(
            "adulthood_arcane_option",
            new TextObject("{=oQJlcPEa}you mastered an element."),
            new TextObject(
                "{=iPyMP3tN}Through rigorous study and practice, you became an expert in a single element—fire, frost, or lightning. Your spells in this domain are potent and feared."
            ),
            new GetNarrativeMenuOptionArgsDelegate(args =>
            {
                args.SetAffectedSkills(new SkillObject[] { CustomSkills.Instance.Destruction });
                args.SetFocusToSkills(FOCUS_TO_ADD);
                args.SetLevelToSkills(SKILL_LEVEL_TO_ADD);
                args.SetLevelToAttribute(Attributes.Instance.Magic, SKILL_LEVEL_TO_ADD);
            }),
            new NarrativeMenuOptionOnConditionDelegate(args => true),
            null,
            null
        );
    }
}
