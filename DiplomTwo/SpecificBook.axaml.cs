using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;
using System.Linq;
using DiplomTwo.Models;

namespace DiplomTwo;

public partial class SpecificBook : Window
{
    public SpecificBook()
    {
        InitializeComponent();
    }
    public SpecificBook(int idForBook)
    {
        InitializeComponent();
        try
        {
            SpecificBookIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
        CallBaza();
        foreach(Book book in ListsStaticClass.listAllBooks)
        {
            if(book.Id ==  idForBook)
            {
                imageForBook.Source = book.CoverBitmap;
                titleForBook.Text = book.Title;
                ratingForBook.Text = book.Rating.ToString();
                contetForBook.Text = book.Description;
                colPagesForBook.Text = book.PageCount.ToString();
                ageForBook.Text = book.AgeLimit.ToString();
                kolReadForBook.Text = book.Kolread.ToString();
                kolPlanForBook.Text = book.Kolplan.ToString();
                kolRewForBook.Text = book.Kolrev.ToString();
                break;
            }
        }
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
            BookGenres = book.BookGenres.ToList(),
        }).ToList();

        ListsStaticClass.listAllAuthors.Clear();
        ListsStaticClass.listAllAuthors = Baza.DbContext.Authors.Select(author => new Author
        {
            Id = author.Id,
            Biography = author.Biography,
            Name = author.Name,
        }).ToList();

        
    }

    private void Back(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }

    private void OpenBooks(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
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