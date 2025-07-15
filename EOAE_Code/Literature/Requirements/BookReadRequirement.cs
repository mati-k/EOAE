using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;

namespace EOAE_Code.Literature.Requirements;

public abstract class BookReadRequirement
{
    public abstract bool Satisfies(Hero hero);

    public abstract void AddTooltips(ItemMenuVM instance, Hero hero);

    public abstract string GetExplanation(Hero hero);
}
