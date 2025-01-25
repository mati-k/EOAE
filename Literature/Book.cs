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

    public Book(BookDataXml bookData)
    {
        ReadTime = bookData.ReadTime;
        ItemName = bookData.ItemName;

        foreach (var readEffect in bookData.ReadEffects)
        {
            if (readEffect is UnlockSpellData unlockSpellData)
                bookReadEffects.Add(new UnlockSpellEffect(unlockSpellData));
            else if (readEffect is IncreaseSkillData increaseSkillData)
                bookReadEffects.Add(new IncreaseSkillEffect(increaseSkillData));
        }
    }

    public void FinishReading(Hero hero)
    {
        var bookItem = MBObjectManager.Instance.GetObject<ItemObject>(ItemName);
        InformationManager.DisplayMessage(
            new InformationMessage(
                $"{hero.Name} has finished reading {bookItem.Name}!",
                UIColors.PositiveIndicator
            )
        );
        SoundEvent.PlaySound2D("event:/ui/notification/unlock");

        bookReadEffects.ForEach(effect => effect.Apply(hero));
    }

    public void AddTooltips(ItemMenuVM itemMenu)
    {
        AddCurrentReaderTooltip(itemMenu);
        bookReadEffects.ForEach(effect => effect.AddTooltips(itemMenu));
        AddPlayerReadStatus(itemMenu);
    }

    private void AddCurrentReaderTooltip(ItemMenuVM itemMenu)
    {
        var literature = Campaign.Current.GetCampaignBehavior<LiteratureCampaignBehavior>();
        var reader = literature.GetBookReader(ItemName);
        if (reader == null)
            return;

        var progress = literature.GetProgress(reader, ItemName);
        var isFinished = literature.HasReadBook(reader, ItemName);
        var progressText = isFinished ? "Read" : $"{progress / ReadTime:0%}";

        itemMenu.AddTooltip("Used by: ", reader.Name.ToString(), Color.Black);
        itemMenu.AddTooltip(
            "Progress: ",
            progressText,
            isFinished ? UIColors.PositiveIndicator : Color.Black
        );
    }

    private void AddPlayerReadStatus(ItemMenuVM itemMenu)
    {
        var literature = Campaign.Current.GetCampaignBehavior<LiteratureCampaignBehavior>();

        if (literature.HasReadBook(Hero.MainHero, ItemName))
            itemMenu.AddTooltip(" ", "Read", Color.FromUint(0xFFBF40BF), 1);
        else
            itemMenu.AddTooltip(" ", "Not read", Color.FromUint(0xFF702963), 1);
    }
}
