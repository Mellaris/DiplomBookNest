using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DiplomTwo.Models;
using System.Collections.Generic;
using System.Linq;

namespace DiplomTwo;

public partial class Books : Window
{
    private List<Book> booksForGenre = new List<Book>();
    public Books()
    {
        InitializeComponent();
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

    private void ListForBooks()
    {
        allBooks.ItemsSource = ListsStaticClass.listAllBooks.ToList();
    }

    private void NewGenreSelection(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        if (genreForBooks.SelectedItem is Genre selectedGenre)
        {
            int selectedGenreId = selectedGenre.Id;

            booksForGenre = ListsStaticClass.listAllBooks
                .Where(book => book.BookGenres.Any(g => g.Id == selectedGenreId))
                .ToList();

            allBooks.ItemsSource = booksForGenre.ToList();
        }
        else
        {
            // Если ничего не выбрано — показать все книги
            allBooks.ItemsSource = ListsStaticClass.listAllBooks.ToList();
        }
    }

    private void NewRatingSelection(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {

    }
}