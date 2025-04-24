using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;
using System.Linq;

namespace DiplomTwo;

public partial class MyBooks : Window
{
    public MyBooks()
    {
        InitializeComponent();
        try
        {
            MyBooksIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
        LoadUserBooks();
    }
    private void LoadUserBooks()
    {
        int currentUserId = ListsStaticClass.currentAccount;

        var userBooks = (from lib in ListsStaticClass.listAllPersonallLibrary
                         where lib.ReaderId == currentUserId  // ‘ильтруем книги по текущему пользователю
                         join book in ListsStaticClass.listAllBooks on lib.BookId equals book.Id
                         join bookAuthor in ListsStaticClass.listAllBookAuthors on book.Id equals bookAuthor.BookId into baGroup
                         from bookAuthor in baGroup.DefaultIfEmpty()
                         join author in ListsStaticClass.listAllAuthors on bookAuthor?.AuthorId equals author?.Id into aGroup
                         from author in aGroup.DefaultIfEmpty()
                         join appAuthor in ListsStaticClass.listAllAppAuthors on bookAuthor?.AppAuthorId equals appAuthor?.Id into appAGroup
                         from appAuthor in appAGroup.DefaultIfEmpty()
                         join user in ListsStaticClass.listAllUsers on appAuthor?.Id equals user?.Id into userGroup
                         from user in userGroup.DefaultIfEmpty()
                         select new BookDisplayModel
                         {
                             BookId = book.Id, // добавл€ем BookId
                             Title = book.Title,
                             Synopsis = book.Synopsis,
                             Quote = book.Quote,
                             CoverBitmap = book.CoverBitmap,
                             // »м€ автора Ч если автор из таблицы Author, используем его им€,
                             // если это AppAuthor, то используем им€ из таблицы Users
                             AuthorName = author?.Name ?? user?.Login ?? "јвтор неизвестен",
                             Rating = lib.Rating,
                             PlotRating = lib.PlotRating,
                             CharactersRating = lib.CharactersRating,
                             WorldRating = lib.WorldRating,
                             RomanceRating = lib.RomanceRating,
                             HumorRating = lib.HumorRating,
                             MeaningRating = lib.MeaningRating
                         }).ToList();

        allMyBooks.ItemsSource = userBooks;
    }
}