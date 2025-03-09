using System.Collections.Generic;
using EOAE_Code.Data.Managers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.SaveSystem;

namespace EOAE_Code.Literature;

public class LiteratureCampaignBehavior : CampaignBehaviorBase
{
    // Dictionary<heroName, Dictionary<bookName, progress>>
    private Dictionary<string, Dictionary<string, float>> bookReadingProgress = new();

    // Dictionary<heroName, bookName>
    private Dictionary<string, string> bookReaders = new();

    public override void SyncData(IDataStore dataStore)
    {
        dataStore.SyncData("bookReadingProgress", ref bookReadingProgress);
        dataStore.SyncData("bookReaders", ref bookReaders);
    }

    public override void RegisterEvents()
    {
        CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, AddDialogs);
        CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, OnHourTick);
    }

    private void AddDialogs(CampaignGameStarter starter)
    {
        LiteratureDialogs.AddBookDialogs(starter);
    }

    private void OnHourTick()
    {
        if (!Campaign.Current.IsMainPartyWaiting)
            return;

        var playerBook = GetReadableBookForPlayer();
        if (playerBook != null)
        {
            ReadTick(Hero.MainHero, playerBook);
        }

        Hero.AllAliveHeroes.ForEach(hero =>
        {
            if (hero.PartyBelongedTo != Hero.MainHero.PartyBelongedTo)
                return;

            if (!bookReaders.TryGetValue(hero.StringId, out var bookName))
                return;

            if (HasReadBook(hero, bookName))
                return;

            ReadTick(hero, BookManager.GetBook(bookName));
        });
    }

    private void ReadTick(Hero hero, Book book)
    {
        var heroId = hero.StringId;
        var bookName = book.ItemName;

        if (!bookReadingProgress.ContainsKey(heroId))
            bookReadingProgress[heroId] = new Dictionary<string, float>();

        var newProgress = GetProgress(hero, bookName) + CalculateReadingProgress(hero, book);
        bookReadingProgress[heroId][bookName] = newProgress;
        if (newProgress >= book.ReadTime)
        {
            book.FinishReading(hero);
            bookReaders.Remove(heroId);
        }
    }

    // Ideally, we would want treat player as any other hero with an ability to select which book to read
    public Book? GetReadableBookForPlayer()
    {
        var inventory = Hero.MainHero.PartyBelongedTo.ItemRoster;
        foreach (var rosterElement in inventory)
        {
            var item = rosterElement.EquipmentElement.Item;

            if (item.Type != ItemObject.ItemTypeEnum.Book)
                continue;

            if (HasReadBook(Hero.MainHero, item.StringId))
                continue;

            if (bookReaders.ContainsValue(item.StringId))
                continue;

            if (!BookManager.IsBook(item.StringId))
                continue;

            var book = BookManager.GetBook(item.StringId);
            if (!book.CanBeReadBy(Hero.MainHero))
                continue;

            return book;
        }

        return null;
    }

    public string? GetCurrentBook(Hero hero)
    {
        if (!bookReaders.TryGetValue(hero.StringId, out var bookName))
            return null;

        return bookName;
    }

    public void StartReading(Hero hero, string bookName)
    {
        bookReaders[hero.StringId] = bookName;
    }

    public void StopReading(Hero hero)
    {
        bookReaders.Remove(hero.StringId);
    }

    public Hero? GetBookReader(string bookName)
    {
        var playerBook = GetReadableBookForPlayer();
        if (playerBook != null && playerBook.ItemName == bookName)
            return Hero.MainHero;

        foreach (var entry in bookReaders)
        {
            if (entry.Value == bookName)
            {
                return Hero.FindFirst(it => it.StringId == entry.Key);
            }
        }

        return null;
    }

    public bool HasReadBook(Hero hero, string itemName)
    {
        if (!bookReadingProgress.ContainsKey(hero.StringId))
            return false;

        if (!bookReadingProgress[hero.StringId].ContainsKey(itemName))
            return false;

        var currentProgress = bookReadingProgress[hero.StringId][itemName];
        return currentProgress >= BookManager.GetBook(itemName).ReadTime;
    }

    public bool CanReadBook(Hero hero, string itemName, out string explanation)
    {
        return BookManager.GetBook(itemName).CanBeReadBy(hero, out explanation);
    }

    public float GetProgress(Hero hero, string bookName)
    {
        if (!bookReadingProgress.ContainsKey(hero.StringId))
            return 0;

        if (!bookReadingProgress[hero.StringId].ContainsKey(bookName))
            return 0;

        return bookReadingProgress[hero.StringId][bookName];
    }

    private static float CalculateReadingProgress(Hero hero, Book book)
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
        ConstructContainerDefinition(typeof(Dictionary<string, string>));
        ConstructContainerDefinition(typeof(Dictionary<string, float>));
        ConstructContainerDefinition(typeof(Dictionary<string, Dictionary<string, float>>));
    }
}
