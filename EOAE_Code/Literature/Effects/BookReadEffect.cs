using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;

namespace EOAE_Code.Literature.Effects;

public abstract class BookReadEffect
{
    public abstract void Apply(Hero hero);

    public abstract void AddTooltips(ItemMenuVM instance);
}
