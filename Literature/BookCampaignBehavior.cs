using System.Collections.Generic;
using EOAE_Code.Data.Managers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace EOAE_Code.Literature;

public class BookCampaignBehavior : CampaignBehaviorBase
{
    private static readonly Dictionary<string, float> BookReadingProgress = new();

    public BookCampaignBehavior()
    {
        BookReadingProgress.Clear();
    }

    public override void RegisterEvents()
    {
        CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, OnHourTick);
    }

    public override void SyncData(IDataStore dataStore)
    {
        // Save the reading progress of each book
    }

    public static float GetProgress(string bookName)
    {
        return BookReadingProgress.ContainsKey(bookName) ? BookReadingProgress[bookName] : 0;
    }

    public static bool IsBookFinished(string itemName)
    {
        return BookReadingProgress.ContainsKey(itemName)
            && BookReadingProgress[itemName] >= BookManager.GetBook(itemName).ReadTime;
    }

    private static void OnHourTick()
    {
        if (!Campaign.Current.IsMainPartyWaiting)
            return;

        var book = GetReadableBookFromInventory();
        if (book == null)
            return;

        BookReadingProgress.TryGetValue(book.ItemName, out var currentProgress);
        BookReadingProgress[book.ItemName] = currentProgress + CalculateReadingProgress();

        if (BookReadingProgress[book.ItemName] >= book.ReadTime)
        {
            book.FinishReading();
        }
    }

    private static Book? GetReadableBookFromInventory()
    {
        var inventory = Hero.MainHero.PartyBelongedTo.ItemRoster;
        var bookIdx = inventory.FindIndex(item =>
            item.Type == ItemObject.ItemTypeEnum.Book && !IsBookFinished(item.StringId)
        );

        if (bookIdx == -1)
            return null;

        var bookItem = inventory.GetItemAtIndex(bookIdx);
        if (!BookManager.IsBook(bookItem.StringId))
            return null;

        return BookManager.GetBook(bookItem.StringId);
    }

    private static float CalculateReadingProgress()
    {
        var progress = 1f;
        if (MobileParty.MainParty.CurrentSettlement != null)
        {
            progress *= 1.25f;
        }

        // We can add more modifiers here, like hero's intelligence, what type of settlement they're in, etc.

        return progress;
    }
}
