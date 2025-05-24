using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using DiplomTwo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiplomTwo;

public partial class AuthorBoard : Window
{
    private List<Book> booksMyAll = new List<Book>();
    public AuthorBoard()
    {
        InitializeComponent();
        try
        {
            AuthorBoardIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
        CallBaza();
        SortForAppList();
    }
    private void BooksListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {

        if (myBooks.SelectedItem != null)
        {
            // Получаем выбранную книгу
            var selectedBook = myBooks.SelectedItem as Book;

            // Проверяем, что книга выбрана
            if (selectedBook != null)
            {
                int bookId = selectedBook.Id; // Получаем id книги

                // Открываем окно для написания главы
                new CreatingBook(bookId).Show();
                Close();
            }
        }

    }
    private void SortForAppList()
    {
        booksMyAll.Clear();

        // Получаем все id книг, написанных текущим автором
        var myBookIds = ListsStaticClass.listAllBookAuthors
            .Where(x => x.AppAuthorId == ListsStaticClass.currentAccount)
            .Select(x => x.BookId)
            .ToList();

        // Отбираем книги, которые электронные и принадлежат текущему автору
        booksMyAll = ListsStaticClass.listAllBooks
            .Where(x => x.IsAuthorBook == true && myBookIds.Contains(x.Id))
            .ToList();

        myBooks.ItemsSource = booksMyAll;
        countAllThisAuthor.Text = booksMyAll.Count.ToString();
    }
    private void CallBaza()
    {
        ListsStaticClass.listAllBooks.Clear();
        ListsStaticClass.listAllBooks = Baza.DbContext.Books.Select(book => new Book
        {
            Id = book.Id,
            Title = book.Title,
            Rating = book.Rating,
            Kolread = book.Kolread,
            Kolplan = book.Kolplan,
            Kolrev = book.Kolrev,
            AgeLimit = book.AgeLimit,
            PageCount = book.PageCount,
            CoverImage = book.CoverImage,
            Synopsis = book.Synopsis,
            Description = book.Description,
            IsAuthorBook = book.IsAuthorBook,
            Dateadd = book.Dateadd,
            SeriesId = book.SeriesId,
        }).ToList();

        ListsStaticClass.listAllBookAuthors.Clear();
        ListsStaticClass.listAllBookAuthors = Baza.DbContext.Bookauthors.Select(bookAuthors => new Bookauthor
        {
            Id = bookAuthors.Id,
            BookId = bookAuthors.BookId,
            AuthorId = bookAuthors.AuthorId,
            AppAuthorId = bookAuthors.AppAuthorId,
        }).ToList();

        ListsStaticClass.listAllComment.Clear();
        ListsStaticClass.listAllComment = Baza.DbContext.Comments.Select(com => new Comment
        {
            Id = com.Id,
            BookId = com.BookId,
            ChapterId = com.ChapterId,
            ReaderId = com.ReaderId,
        }).ToList();

        ListsStaticClass.listAllPersonallLibrary.Clear();
        ListsStaticClass.listAllPersonallLibrary = Baza.DbContext.Personallibraries.Select(pl => new Personallibrary
        {
            Id = pl.Id,
            BookId = pl.BookId,
            ReaderId = pl.ReaderId,
        }).ToList();
    }
    private void TopTypeChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (comboTopType.SelectedIndex == 0)
        {
            ShowTopByComments();
        }
        else if (comboTopType.SelectedIndex == 1)
        {
            ShowTopByRating();
        }
        else if (comboTopType.SelectedIndex == 2)
        {
            ShowTopByReadCount();
        }
    }
    private void ShowTopByComments()
    {
        var myBookIds = ListsStaticClass.listAllBookAuthors
            .Where(x => x.AppAuthorId == ListsStaticClass.currentAccount)
            .Select(x => x.BookId)
            .ToList();

        // Сначала получаем книги с их количеством комментариев
        var topBookCommentCounts = ListsStaticClass.listAllComment
            .Where(c => c.BookId.HasValue && myBookIds.Contains(c.BookId.Value))
            .GroupBy(c => c.BookId.Value)
            .Select(g => new
            {
                Book = ListsStaticClass.listAllBooks.FirstOrDefault(b => b.Id == g.Key),
                CommentCount = g.Count()
            })
            .Where(x => x.Book != null)
            .OrderByDescending(x => x.CommentCount)
            .Take(3)
            .Select(x => x.Book)
            .ToList();

        listSort.ItemsSource = topBookCommentCounts;
    }
    private void ShowTopByRating()
    {
        var top = ListsStaticClass.listAllBooks
            .Where(x => x.IsAuthorBook == true)
            .OrderByDescending(x => x.Rating)
            .Take(3)
            .ToList();

        listSort.ItemsSource = top;
    }

    private void ShowTopByReadCount()
    {
        var myBookIds = ListsStaticClass.listAllBookAuthors
            .Where(x => x.AppAuthorId == ListsStaticClass.currentAccount)
            .Select(x => x.BookId)
            .ToList();

        var topReadBooks = ListsStaticClass.listAllPersonallLibrary
            .Where(pl => myBookIds.Contains(pl.BookId))
            .GroupBy(pl => pl.BookId)
            .Select(g => new
            {
                Book = ListsStaticClass.listAllBooks.FirstOrDefault(b => b.Id == g.Key),
                ReadCount = g.Count()
            })
            .Where(x => x.Book != null)
            .OrderByDescending(x => x.ReadCount)
            .Take(3)
            .Select(x => x.Book)
            .ToList();

        listSort.ItemsSource = topReadBooks;
    }


    private void AddNewBook(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new BookDownload().Show();
        Close();
    }

    private void BackAccount(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new personalAccount().Show();
        Close();
    }

    private void Home(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }

    private void BooksAll(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Books().Show();
        Close();
    }

    private void OpenAuthors(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new AllAuthors().Show();
        Close();
    }

    private void OpenNews(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Headline().Show();
        Close();
    }

    private void MyAc(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new personalAccount().Show();
        Close();
    }
}