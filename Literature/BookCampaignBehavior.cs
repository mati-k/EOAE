using System.Collections.Generic;
using EOAE_Code.Data.Managers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.SaveSystem;

namespace EOAE_Code.Literature;

public class BookCampaignBehavior : CampaignBehaviorBase
{
    private static Dictionary<string, float> bookReadingProgress = new();

    public BookCampaignBehavior()
    {
        bookReadingProgress.Clear();
    }

    public override void RegisterEvents()
    {
        CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, OnHourTick);
    }

    public override void SyncData(IDataStore dataStore)
    {
        dataStore.SyncData("BookReadingProgress", ref bookReadingProgress);
    }

    public static float GetProgress(string bookName)
    {
        return bookReadingProgress.ContainsKey(bookName) ? bookReadingProgress[bookName] : 0;
    }

    public static bool IsBookFinished(string itemName)
    {
        return bookReadingProgress.ContainsKey(itemName)
            && bookReadingProgress[itemName] >= BookManager.GetBook(itemName).ReadTime;
    }

    private static void OnHourTick()
    {
        if (!Campaign.Current.IsMainPartyWaiting)
            return;

        var book = GetReadableBookFromInventory();
        if (book == null)
            return;

        bookReadingProgress.TryGetValue(book.ItemName, out var currentProgress);
        bookReadingProgress[book.ItemName] = currentProgress + CalculateReadingProgress();

        if (bookReadingProgress[book.ItemName] >= book.ReadTime)
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

public class BookProgressSaveDefiner : SaveableTypeDefiner
{
    // use a big number and ensure that no other mod is using a close range
    public BookProgressSaveDefiner()
        : base(5_275_032) { }

    protected override void DefineContainerDefinitions()
    {
        ConstructContainerDefinition(typeof(Dictionary<string, float>));
    }
}
