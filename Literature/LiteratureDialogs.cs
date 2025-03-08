using EOAE_Code.Data.Loaders;
using EOAE_Code.Data.Managers;
using EOAE_Code.Data.Xml.Book;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace EOAE_Code.Literature;

public static class LiteratureDialogs
{
    private static LiteratureCampaignBehavior Literature =>
        Campaign.Current.GetCampaignBehavior<LiteratureCampaignBehavior>();

    public static void AddBookDialogs(CampaignGameStarter starter)
    {
        if (BookManager.GetBookNames().Count == 0)
            XmlDataLoader.LoadXmlDataList<BookDataXml, BookManager>("books.xml");

        starter.AddPlayerLine(
            "companion_start_book",
            "hero_main_options",
            "companion_book_start_talk",
            "About your studies...",
            null,
            null
        );
        starter.AddDialogLine(
            "companion_start_book_response",
            "companion_book_start_talk",
            "companion_book_choose",
            "{COMPANION_READING}",
            LoadCompanionReadingData,
            null
        );

        foreach (var bookName in BookManager.GetBookNames())
        {
            starter.AddPlayerLine(
                $"companion_talk_read_book_{bookName}",
                "companion_book_choose",
                $"companion_book_read_{bookName}",
                $"Read \"{{BOOK_NAME_{bookName}}}\" {{CURRENT_READER_{bookName}}}",
                () => LoadBookData(bookName) && IsBookInInventory(bookName),
                null,
                100,
                (out TextObject explanation) =>
                {
                    var hero = Hero.OneToOneConversationHero;
                    if (Literature.HasReadBook(hero, bookName))
                    {
                        explanation = new TextObject($"{hero.Name} has already read this book.");
                        return false;
                    }

                    if (!Literature.CanReadBook(hero, bookName, out var reqExplanation))
                    {
                        explanation = new TextObject(reqExplanation);
                        return false;
                    }

                    explanation = new TextObject();
                    return true;
                }
            );
            starter.AddDialogLine(
                $"companion_talk_read_book_response_{bookName}",
                $"companion_book_read_{bookName}",
                "hero_main_options",
                $"Okay, I will read {{BOOK_NAME_{bookName}}}.",
                null,
                () => StartReading(bookName)
            );
        }

        starter.AddPlayerLine(
            "companion_talk_stop_reading",
            "companion_book_choose",
            "companion_okay",
            "I want you to stop reading.",
            IsCompanionReading,
            StopReading
        );
        starter.AddPlayerLine(
            "companion_book_end_talk",
            "companion_book_choose",
            "companion_okay",
            "Never mind.",
            null,
            null
        );
    }

    private static bool LoadBookData(string bookName)
    {
        var bookItem = MBObjectManager.Instance.GetObject<ItemObject>(bookName);
        var bookReader = Literature.GetBookReader(bookName);
        MBTextManager.SetTextVariable($"BOOK_NAME_{bookName}", bookItem.Name);
        MBTextManager.SetTextVariable(
            $"CURRENT_READER_{bookName}",
            bookReader == null ? "" : $"({bookReader.Name})"
        );

        return true;
    }

    private static bool LoadCompanionReadingData()
    {
        var currentlyReading = Literature.GetCurrentBook(Hero.OneToOneConversationHero);

        if (currentlyReading == null)
        {
            MBTextManager.SetTextVariable(
                "COMPANION_READING",
                "Yes? I'm not reading anything right now."
            );
        }
        else
        {
            LoadBookData(currentlyReading);
            MBTextManager.SetTextVariable(
                "COMPANION_READING",
                $"Right. I'm currently reading {{BOOK_NAME_{currentlyReading}}}."
            );
        }

        return true;
    }

    private static bool IsCompanionReading()
    {
        return Literature.GetCurrentBook(Hero.OneToOneConversationHero) != null;
    }

    private static void StopReading()
    {
        Literature.StopReading(Hero.OneToOneConversationHero);
    }

    private static void StartReading(string bookName)
    {
        Literature.StartReading(Hero.OneToOneConversationHero, bookName);
    }

    private static bool IsBookInInventory(string bookName)
    {
        return MobileParty.MainParty.ItemRoster.FindIndex(item => item.StringId == bookName) != -1;
    }
}
