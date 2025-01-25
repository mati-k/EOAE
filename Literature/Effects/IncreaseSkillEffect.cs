using System;
using EOAE_Code.Character;
using EOAE_Code.Data.Xml.Book;
using EOAE_Code.Extensions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace EOAE_Code.Literature.Effects;

public class IncreaseSkillEffect : BookReadEffect
{
    private readonly string skillName;

    private SkillObject Skill
    {
        get
        {
            return skillName switch
            {
                "OneHanded" => DefaultSkills.OneHanded,
                "TwoHanded" => DefaultSkills.TwoHanded,
                "Polearm" => DefaultSkills.Polearm,
                "Bow" => DefaultSkills.Bow,
                "Crossbow" => DefaultSkills.Crossbow,
                "Throwing" => DefaultSkills.Throwing,
                "Riding" => DefaultSkills.Riding,
                "Athletics" => DefaultSkills.Athletics,
                "Crafting" => DefaultSkills.Crafting,
                "Tactics" => DefaultSkills.Tactics,
                "Scouting" => DefaultSkills.Scouting,
                "Roguery" => DefaultSkills.Roguery,
                "Charm" => DefaultSkills.Charm,
                "Trade" => DefaultSkills.Trade,
                "Steward" => DefaultSkills.Steward,
                "Leadership" => DefaultSkills.Leadership,
                "Medicine" => DefaultSkills.Medicine,
                "Engineering" => DefaultSkills.Engineering,
                "Destruction" => CustomSkills.Instance.Destruction,
                "Restoration" => CustomSkills.Instance.Restoration,
                "Conjuration" => CustomSkills.Instance.Conjuration,
                _ => throw new Exception("Invalid skill name: " + skillName)
            };
        }
    }

    public IncreaseSkillEffect(IncreaseSkillData data)
    {
        skillName = data.Skill;
    }

    public override void Apply(Hero hero)
    {
        InformationManager.DisplayMessage(
            new InformationMessage(
                $"{hero.Name} is now more experienced in {Skill.Name}!",
                UIColors.PositiveIndicator.AddFactorInHSB(0, -0.3f, 0)
            )
        );
        // hero.AddSkillXp(CustomSkills.Instance.Destruction, 100);
    }

    public override void AddTooltips(ItemMenuVM instance)
    {
        instance.AddTooltip("Improves: ", Skill.Name.ToString(), Color.Black);
    }
}
