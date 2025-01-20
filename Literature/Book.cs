using System.Collections.Generic;
using EOAE_Code.Data.Xml.Book;
using EOAE_Code.Extensions;
using EOAE_Code.Literature.Effects;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace EOAE_Code.Literature;

public class Book
{
    public int ReadTime { get; }

    public string ItemName { get; }

    private readonly List<BookReadEffect> bookReadEffects = new();
    private readonly ItemObject bookItem;

    public Book(BookDataXml bookData)
    {
        ReadTime = bookData.ReadTime;
        ItemName = bookData.ItemName;
        bookItem = MBObjectManager.Instance.GetObject<ItemObject>(ItemName);

        foreach (var readEffect in bookData.ReadEffects)
        {
            if (readEffect is UnlockSpellData unlockSpellData)
                bookReadEffects.Add(new UnlockSpellEffect(unlockSpellData));
            else if (readEffect is IncreaseSkillData increaseSkillData)
                bookReadEffects.Add(new IncreaseSkillEffect(increaseSkillData));
        }
    }

    public void FinishReading()
    {
        InformationManager.DisplayMessage(
            new InformationMessage($"Finished reading {bookItem.Name}!", UIColors.PositiveIndicator)
        );
        SoundEvent.PlaySound2D("event:/ui/notification/unlock");

        foreach (var readEffect in bookReadEffects)
        {
            readEffect.Apply(Campaign.Current.MainParty.LeaderHero);
        }
    }

    public void AddTooltips(ItemMenuVM itemMenu, SPItemVM item)
    {
        var progress = BookCampaignBehavior.GetProgress(item.StringId);
        var isFinished = BookCampaignBehavior.IsBookFinished(item.StringId);
        var progressText = isFinished ? "Read" : $"{progress / ReadTime:0%}";

        itemMenu.AddTooltip(
            "Progress: ",
            progressText,
            isFinished ? UIColors.PositiveIndicator : Color.Black
        );

        foreach (var readEffect in bookReadEffects)
        {
            readEffect.AddTooltips(itemMenu);
        }
    }
}
