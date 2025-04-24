using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DiplomTwo.Models;
using System.Collections.Generic;
using System.Linq;

namespace DiplomTwo;

public partial class AuthorBoard : Window
{
    private List<Book> booksMyAll = new List<Book>();
    public AuthorBoard()
    {
        InitializeComponent();
        CallBaza();
        SortForAppList();
    }
    private void SortForAppList()
    {
        booksMyAll.Clear();
        booksMyAll = ListsStaticClass.listAllBooks.Where(x => x.IsAuthorBook == true).ToList();
        myBooks.ItemsSource = booksMyAll.ToList();
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

   
}