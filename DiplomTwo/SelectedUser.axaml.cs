using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using DiplomTwo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiplomTwo;

public partial class SelectedUser : Window
{
    private int _id;
    private List<Book> booksMyAll = new List<Book>();
    public SelectedUser()
    {
        InitializeComponent();
    }
    public SelectedUser(int idThisUser)
    {
        InitializeComponent();
        try
        {
            SelectedUserIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
        _id = idThisUser;
        CallBaza();
        foreach(User user in ListsStaticClass.listAllUsers)
        {
            if(idThisUser == user.Id)
            {
                userName.Text = user.Login;
                break;
            }
        }
        foreach(Reader reader in ListsStaticClass.listAllReaders)
        {
            if(idThisUser == reader.UserId)
            {
                userDescr.Text = reader.ProfileDescription;
                userPhoto.Source = reader.CoverBitmap;
                break;
            }
        }
        TopG();
        CountRAndQ();
        ChooseBook();
        AuthorBooksOpen();
        if(booksMyAll.Count > 0)
        {
            panel.IsVisible = true;
        }
        else
        {
            panel.IsVisible = false;
        }
    }
    private void CallBaza()
    {
        ListsStaticClass.listAllUsers.Clear();
        ListsStaticClass.listAllUsers = Baza.DbContext.Users.ToList();
        ListsStaticClass.listAllReaders.Clear();
        ListsStaticClass.listAllReaders = Baza.DbContext.Readers.ToList();
        ListsStaticClass.listAllPersonallLibrary.Clear();
        ListsStaticClass.listAllPersonallLibrary = Baza.DbContext.Personallibraries.ToList();
        ListsStaticClass.listAllBookGenres.Clear();
        ListsStaticClass.listAllBookGenres = Baza.DbContext.BookGenres.ToList();
        ListsStaticClass.listAllBookreview.Clear();
        ListsStaticClass.listAllBookreview = Baza.DbContext.Bookreviews.ToList();
        ListsStaticClass.listAllQuote.Clear();
        ListsStaticClass.listAllQuote = Baza.DbContext.Quotes.ToList();
        ListsStaticClass.listAllAppAuthors.Clear();
        ListsStaticClass.listAllAppAuthors = Baza.DbContext.Appauthors.ToList();
        ListsStaticClass.listAllBooks.Clear();
        ListsStaticClass.listAllBooks = Baza.DbContext.Books.ToList();
    }
    private void AuthorBooksOpen()
    {
        booksMyAll.Clear();

        // �������� ��� id ����, ���������� ������� �������
        var myBookIds = ListsStaticClass.listAllBookAuthors
            .Where(x => x.AppAuthorId == ListsStaticClass.currentAccount)
            .Select(x => x.BookId)
            .ToList();

        // �������� �����, ������� ����������� � ����������� �������� ������
        booksMyAll = ListsStaticClass.listAllBooks
            .Where(x => x.IsAuthorBook == true && myBookIds.Contains(x.Id))
            .ToList();

        myBooks.ItemsSource = booksMyAll;
    }
    private void ChooseBook()
    {
        var userBooks = ListsStaticClass.listAllPersonallLibrary
           .Where(lib => lib.ReaderId == _id)
           .Select(lib =>
           {
               var book = ListsStaticClass.listAllBooks.FirstOrDefault(b => b.Id == lib.BookId);
               if (book == null) return null;

               var bookAuthor = ListsStaticClass.listAllBookAuthors
                   .FirstOrDefault(ba => ba.BookId == book.Id);

               string authorName = "����� ����������";

               if (book.IsAuthorBook)
               {
                   var appAuthor = ListsStaticClass.listAllAppAuthors
                       .FirstOrDefault(aa => aa.Id == bookAuthor?.AppAuthorId);

                   var user = ListsStaticClass.listAllUsers
                       .FirstOrDefault(u => u.Id == appAuthor?.Id);

                   authorName = user?.Login ?? "����� ����������";
               }
               else
               {
                   var author = ListsStaticClass.listAllAuthors
                       .FirstOrDefault(a => a.Id == bookAuthor?.AuthorId);

                   authorName = author?.Name ?? "����� ����������";
               }

               return new BookDisplayModel
               {
                   BookId = book.Id,
                   Title = book.Title,
                   Synopsis = book.Synopsis,
                   Quote = book.Quote,
                   CoverBitmap = book.CoverBitmap,
                   AuthorName = authorName,
                   Rating = lib.Rating,
                   PlotRating = lib.PlotRating,
                   CharactersRating = lib.CharactersRating,
                   WorldRating = lib.WorldRating,
                   RomanceRating = lib.RomanceRating,
                   HumorRating = lib.HumorRating,
                   MeaningRating = lib.MeaningRating
               };
           })
           .Where(x => x != null)
           .ToList();
        allBookRead.ItemsSource = userBooks.ToList();
        userBookCount.Text = userBooks.Count.ToString();
    }
    private void CountRAndQ()
    {
        int userRevCount = ListsStaticClass.listAllBookreview.Where(x => x.ReaderId == _id).Count();
        int userQCount = ListsStaticClass.listAllQuote.Where(y => y.ReaderId == _id).Count();
        userQ.Text = userQCount.ToString();
        userRev.Text = userRevCount.ToString();
    }
    private void TopG()
    {
        var userLibraryBooks = ListsStaticClass.listAllPersonallLibrary
            .Where(pl => pl.ReaderId == _id)
            .Select(pl => pl.BookId)
            .Distinct()
            .ToList();

        // �������� ��� ����� ���� ���� (������ �� ������)
        var genresCount = new Dictionary<int, int>(); // id ����� -> ������� ��� �����������

        foreach (var bookId in userLibraryBooks)
        {
            var genreIds = ListsStaticClass.listAllBookGenres
                .Where(bg => bg.BookId == bookId)
                .Select(bg => bg.GenreId);

            foreach (var genreId in genreIds)
            {
                if (genresCount.ContainsKey(genreId))
                    genresCount[genreId]++;
                else
                    genresCount[genreId] = 1;
            }
        }

        // ������� ���-3 �����
        var topGenres = genresCount
            .OrderByDescending(g => g.Value)
            .Take(3)
            .ToList();

        // ��������� �������� � ���������� � ����������
        string? topGenreName1 = null, topGenreName2 = null, topGenreName3 = null;
        int topGenreCount1 = 0, topGenreCount2 = 0, topGenreCount3 = 0;

        for (int i = 0; i < topGenres.Count; i++)
        {
            var genreId = topGenres[i].Key;
            var count = topGenres[i].Value;
            var genreName = ListsStaticClass.listAllGenres.FirstOrDefault(g => g.Id == genreId)?.Name;

            if (i == 0) { topGenreName1 = genreName; topGenreCount1 = count; }
            else if (i == 1) { topGenreName2 = genreName; topGenreCount2 = count; }
            else if (i == 2) { topGenreName3 = genreName; topGenreCount3 = count; }
        }
        userGenerOne.Text = topGenreName2;
        userGenerTwo.Text = topGenreName1;
        userGenerThree.Text = topGenreName3;
    }
    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new personalAccount().Show();
        Close();
    }

    private void OpenFr(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Friends().Show();
        Close();
    }

    private void BookOpen(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Books().Show();
        Close();
    }

    private void HomeOpen(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }
}