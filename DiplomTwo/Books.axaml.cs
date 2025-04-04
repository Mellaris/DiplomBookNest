using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DiplomTwo.Models;
using System.Linq;

namespace DiplomTwo;

public partial class Books : Window
{
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
            Description = genre.Description,
        }).ToList();

        genreForBooks.ItemsSource = ListsStaticClass.listAllGenres;
    }

    private void ListForBooks()
    {
        allBooks.ItemsSource = ListsStaticClass.listAllBooks.ToList();
    }

    private void NewGenreSelection(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {

    }

    private void NewRatingSelection(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
    }
}