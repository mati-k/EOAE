using EOAE_Code.Data.Managers;
using EOAE_Code.Data.Xml.Book;
using EOAE_Code.Extensions;
using EOAE_Code.Magic.Spells;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Library;

namespace EOAE_Code.Literature.Effects;

public class UnlockSpellEffect : BookReadEffect
{
    private Spell spell;

    public UnlockSpellEffect(UnlockSpellData unlockSpellData)
    {
        spell = SpellManager.GetSpellFromItem(unlockSpellData.Spell);
    }

    public override void Apply(Hero hero)
    {
        InformationManager.DisplayMessage(
            new InformationMessage(
                $"{hero.Name} has unlocked new spell: {spell.Name}!",
                UIColors.PositiveIndicator.AddFactorInHSB(0, -0.3f, 0)
            )
        );
    }

    public override void AddTooltips(ItemMenuVM instance)
    {
        instance.AddTooltip("Unlocks: ", spell.Name, Color.Black);
    }
}
