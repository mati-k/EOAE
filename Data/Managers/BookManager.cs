using System.Collections.Generic;
using EOAE_Code.Data.Xml.Book;
using EOAE_Code.Interfaces;
using EOAE_Code.Literature;

namespace EOAE_Code.Data.Managers;

public class BookManager : IDataManager<BookDataXml>
{
    private static readonly Dictionary<string, Book> Books = new();

    public static bool IsBook(string itemName)
    {
        return Books.ContainsKey(itemName);
    }

    public static Book GetBook(string itemName)
    {
        return Books[itemName];
    }

    public void Add(BookDataXml item)
    {
        var book = new Book(item);
        Books.Add(book.ItemName, book);
    }

    public static void Clear()
    {
        Books.Clear();
    }
}
