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