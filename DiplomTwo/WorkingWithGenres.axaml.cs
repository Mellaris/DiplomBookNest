using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Linq;
using DiplomTwo.Models;

namespace DiplomTwo;

public partial class WorkingWithGenres : Window
{
    private string text;
    private int check = 0;
    public WorkingWithGenres()
    {
        InitializeComponent();
        CallBaza();
        allGenres.ItemsSource = ListsStaticClass.listAllGenres.OrderBy(x => x.Id).ToList();
    }
    private void CallBaza()
    {
        ListsStaticClass.listAllGenres.Clear();
        ListsStaticClass.listAllGenres = Baza.DbContext.Genres.Select(genre => new Genre
        {
            Id = genre.Id,
            Name = genre.Name,
        }).ToList();

        ListsStaticClass.listAllBookGenres.Clear();
        ListsStaticClass.listAllBookGenres = Baza.DbContext.BookGenres.Select(bookG => new BookGenre
        {
            Id = bookG.Id,
            BookId = bookG.BookId,
            GenreId = bookG.GenreId,
        }).ToList();
    }

    private void AddNewGenre(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        check = 0;
        if(!string.IsNullOrEmpty(textForGenre.Text))
        {
            text = textForGenre.Text;
            if (text.Length < 46)
            { 
                foreach(Genre genre in ListsStaticClass.listAllGenres)
                {
                    if(text == genre.Name)
                    {
                        check = 1;
                        string error = "Такой жанр уже есть!";
                        new ErrorReport(error).ShowDialog(this);
                        break;
                    }
                }
                if(check == 0)
                {
                    var newAdd = new Genre
                    {
                        Id = ListsStaticClass.listAllGenres.LastOrDefault().Id + 1,
                        Name = textForGenre.Text,
                    };
                    Baza.DbContext.Genres.Add(newAdd);
                    Baza.DbContext.SaveChanges();
                    CallBaza();
                    allGenres.ItemsSource = null;
                    allGenres.ItemsSource = ListsStaticClass.listAllGenres.OrderBy(x => x.Id).ToList();
                }               
            }
            else
            {
                string error = "Не может быть больше 45 символов!";
                new ErrorReport(error).ShowDialog(this);
            }
        }

    }
    private void Delete(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        int selectId = (int)(sender as Button).Tag;
        bool hasBooks = ListsStaticClass.listAllBookGenres.Any(bg => bg.GenreId == selectId);

        if (hasBooks)
        {
            string errorTwo = "Невозможно удалить жанр, у которого есть книга!";
            new ErrorReport(errorTwo).ShowDialog(this);
            return;
        }

        var thisDelete = Baza.DbContext.Genres.FirstOrDefault(x => x.Id == selectId);
        if (thisDelete != null)
        {
            Baza.DbContext.Genres.Remove(thisDelete);
            Baza.DbContext.SaveChanges();
            CallBaza();
            allGenres.ItemsSource = null;
            allGenres.ItemsSource = ListsStaticClass.listAllGenres.OrderBy(x => x.Id).ToList();
        }
    }
}