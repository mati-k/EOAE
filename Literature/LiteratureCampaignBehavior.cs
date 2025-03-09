using System.Collections.Generic;
using EOAE_Code.Data.Managers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
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
        CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, Initialize);
        CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, OnHourTick);
    }

    private void Initialize(CampaignGameStarter starter)
    {
        LiteratureDialogs.AddBookDialogs(starter);

        starter.AddGameMenuOption(
            "town",
            "select_book",
            "Select a book to read",
            ShouldShowBookMenu,
            OnBookMenuClick
        );
    }

    private bool ShouldShowBookMenu(MenuCallbackArgs args)
    {
        args.optionLeaveType = GameMenuOption.LeaveType.OpenStash;

        return GetBooksInInventory().Count > 0;
    }

    private void OnBookMenuClick(MenuCallbackArgs args)
    {
        var heroElements = new List<InquiryElement>();

        for (var i = 0; i < Hero.MainHero.PartyBelongedTo.MemberRoster.Count; i++)
        {
            var character = Hero.MainHero.PartyBelongedTo.MemberRoster.GetCharacterAtIndex(i);

            var hero = character.HeroObject;
            if (hero == null)
                continue;

            var title = hero.Name.ToString();

            var bookId = GetCurrentBook(hero);
            if (bookId != null)
            {
                var book = MBObjectManager.Instance.GetObject<ItemObject>(bookId);
                title += $" (reading \"{book.Name}\")";
            }

            heroElements.Add(
                new InquiryElement(
                    hero,
                    title,
                    new ImageIdentifier(CharacterCode.CreateFrom(character))
                )
            );
        }

        MBInformationManager.ShowMultiSelectionInquiry(
            new MultiSelectionInquiryData(
                new TextObject("Select a character:").ToString(),
                "",
                heroElements,
                true,
                1,
                1,
                new TextObject("Select").ToString(),
                new TextObject("Leave").ToString(),
                selected =>
                {
                    if (selected.Count == 0)
                        return;

                    if (selected[0].Identifier is not Hero hero)
                        return;

                    DisplayBooksForHero(hero);
                },
                _ => { }
            )
        );
    }

    private void DisplayBooksForHero(Hero hero)
    {
        var bookOptions = new List<InquiryElement>();
        foreach (var bookObject in GetBooksInInventory())
        {
            var book = BookManager.GetBook(bookObject.StringId);
            var progress = GetProgress(hero, bookObject.StringId);
            var title = $"{bookObject.Name} [{progress / book.ReadTime:0%}]";

            var reader = GetBookReader(bookObject.StringId);
            if (reader != null)
            {
                title += $" (being read by {reader.Name})";
            }

            var bookOption = new InquiryElement(
                bookObject.StringId,
                title,
                new ImageIdentifier(bookObject)
            );

            if (HasReadBook(hero, bookObject.StringId))
            {
                bookOption = new InquiryElement(
                    bookObject.StringId,
                    title,
                    new ImageIdentifier(bookObject),
                    false,
                    "You have already read this book"
                );
            }
            else if (!book.CanBeReadBy(hero, out var explanation))
            {
                bookOption = new InquiryElement(
                    bookObject.StringId,
                    title,
                    new ImageIdentifier(bookObject),
                    false,
                    explanation
                );
            }

            bookOptions.Add(bookOption);
        }

        if (GetCurrentBook(hero) != null)
        {
            bookOptions.Add(new InquiryElement("STOP", "Stop reading", null));
        }

        MBInformationManager.ShowMultiSelectionInquiry(
            new MultiSelectionInquiryData(
                "Select a book to read:",
                "",
                bookOptions,
                true,
                1,
                1,
                "Select",
                "Cancel",
                selected =>
                {
                    if (selected.Count == 0)
                        return;

                    if (selected[0].Identifier is not string bookId)
                        return;

                    if (bookId == "STOP")
                    {
                        StopReading(hero);
                        return;
                    }

                    var currentReader = GetBookReader(bookId);
                    if (currentReader != null)
                        StopReading(currentReader);

                    StartReading(hero, bookId);
                },
                _ => { }
            )
        );
    }

    private void OnHourTick()
    {
        if (!Campaign.Current.IsMainPartyWaiting)
            return;

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

    public string? GetCurrentBook(Hero hero)
    {
        if (!bookReaders.TryGetValue(hero.StringId, out var bookId))
            return null;

        return bookId;
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

    private List<ItemObject> GetBooksInInventory()
    {
        var inventory = Hero.MainHero.PartyBelongedTo.ItemRoster;
        var books = new List<ItemObject>();
        foreach (var rosterElement in inventory)
        {
            var item = rosterElement.EquipmentElement.Item;
            if (item.Type == ItemObject.ItemTypeEnum.Book)
            {
                books.Add(item);
            }
        }

        return books;
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
