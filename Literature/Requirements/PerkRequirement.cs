using EOAE_Code.Data.Xml.Book;
using EOAE_Code.Extensions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace EOAE_Code.Literature.Requirements;

public class PerkRequirement : BookReadRequirement
{
    private string PerkId { get; }
    private PerkObject Perk => MBObjectManager.Instance.GetObject<PerkObject>(PerkId);

    public PerkRequirement(PerkRequirementData perkRequirementData)
    {
        PerkId = perkRequirementData.Perk;
    }

    public override bool Satisfies(Hero hero)
    {
        return hero.GetPerkValue(Perk);
    }

    public override void AddTooltips(ItemMenuVM instance, Hero hero)
    {
        instance.AddTooltip(
            new TextObject("{=A0UyM0DS}Requires: ").ToString(),
            Perk.Name.ToString(),
            Satisfies(hero) ? UIColors.PositiveIndicator : UIColors.NegativeIndicator
        );
    }

    public override string GetExplanation(Hero hero)
    {
        return new TextObject("{=KvmhAaaS}Requires: {Requirement}")
            .SetTextVariable("Requirement", Perk.Name)
            .ToString();
    }
}
