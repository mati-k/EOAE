using EOAE_Code.Data.Xml.Book;
using EOAE_Code.Extensions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace EOAE_Code.Literature.Requirements;

public class SkillRequirement : BookReadRequirement
{
    private string SkillId { get; }
    private int Level { get; }
    private SkillObject Skill => MBObjectManager.Instance.GetObject<SkillObject>(SkillId);

    public SkillRequirement(SkillRequirementData skillRequirementData)
    {
        SkillId = skillRequirementData.Skill;
        Level = skillRequirementData.Level;
    }

    public override bool Satisfies(Hero hero)
    {
        return hero.GetSkillValue(Skill) >= Level;
    }

    public override void AddTooltips(ItemMenuVM instance, Hero hero)
    {
        instance.AddTooltip(
            new TextObject("{=A0UyM0DS}Requires: ").ToString(),
            $"{Skill.Name} {Level}",
            Satisfies(hero) ? UIColors.PositiveIndicator : UIColors.NegativeIndicator
        );
    }

    public override string GetExplanation(Hero hero)
    {
        return new TextObject("{=KvmhAaaS}Requires: {Requirement}")
            .SetTextVariable("Requirement", $"{Skill.Name} {Level}")
            .ToString();
    }
}
