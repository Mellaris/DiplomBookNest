using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using DiplomTwo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiplomTwo;

public partial class Books : Window
{
    private List<Book> booksForGenre = new List<Book>();
    private int selectedGenreId;
    private int selectRatingId = -1;
    private string textForSearch;
    public Books()
    {
        InitializeComponent();
        CallBaza();
        try
        {
            BooksIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
        ListForBooks();
        ListForGenre();
    }
    private void ListForGenre()
    {
        ListsStaticClass.listAllGenres.Clear();
        ListsStaticClass.listAllGenres = Baza.DbContext.Genres.Select(genre => new Genre
        {
            Id = genre.Id,
            Name = genre.Name,
        }).ToList();

        genreForBooks.ItemsSource = ListsStaticClass.listAllGenres;
    }
    private void OpenSelectBook(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        int selectIdBookForOpen = (int)(sender as Button).Tag;
        new SpecificBook(selectIdBookForOpen).Show();
        Close();
    }

    private void ListForBooks()
    {
        allBooks.ItemsSource = ListsStaticClass.listAllBooks.ToList();
    }
    private void DisplayForAllFiltr()
    {
        booksForGenre.Clear();
        booksForGenre = ListsStaticClass.listAllBooks.ToList();
        if (selectedGenreId > 0)
        {
            booksForGenre = booksForGenre
               .Where(book => book.BookGenres.Any(g => g.GenreId == selectedGenreId))
               .ToList();           
        }
        if (selectRatingId == 0)
        {
            booksForGenre = booksForGenre.OrderBy(book => book.Rating).ToList();
        }
        else if (selectRatingId == 1)
        {
            booksForGenre = booksForGenre.OrderByDescending(book => book.Rating).ToList();
        }
        if (!string.IsNullOrEmpty(textForSearch))
        {
            booksForGenre = booksForGenre.Where(a => a.Title.ToLower().Contains(textForSearch.ToLower())).ToList();
        }
        allBooks.ItemsSource = booksForGenre.ToList();
    }

    private void NewGenreSelection(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        selectedGenreId = (sender as ComboBox).SelectedIndex;
        selectedGenreId = selectedGenreId + 1;
        DisplayForAllFiltr();
    }

    private void NewRatingSelection(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        selectRatingId = (sender as ComboBox).SelectedIndex;
        DisplayForAllFiltr();
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

        ListsStaticClass.listAllBookGenres.Clear();
        ListsStaticClass.listAllBookGenres = Baza.DbContext.BookGenres.Select(bookGenre => new BookGenre
        {
            Id = bookGenre.Id,
            BookId = bookGenre.BookId,
            GenreId = bookGenre.GenreId,
        }).ToList();
    }

    private void TextBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        string text = (sender as TextBox).Text;
        textForSearch = text;
        DisplayForAllFiltr();
    }

    
}