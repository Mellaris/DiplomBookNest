using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DiplomTwo.Models;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;

namespace DiplomTwo;

public partial class WorkingWithAuthors : Window
{
    private string text;
    private int check = 0;
    public WorkingWithAuthors()
    {
        InitializeComponent();
        CallBaza();
        allAuthors.ItemsSource = ListsStaticClass.listAllAuthors;
    }
    private void CallBaza()
    {
        ListsStaticClass.listAllAuthors.Clear();
        ListsStaticClass.listAllAuthors = Baza.DbContext.Authors.ToList();

        ListsStaticClass.listAllBookAuthors.Clear();
        ListsStaticClass.listAllBookAuthors = Baza.DbContext.Bookauthors.ToList();
    }

    private void AddNewGenre(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        check = 0;
        if (!string.IsNullOrEmpty(textForGenre.Text))
        {
            text = textForGenre.Text;
            if (text.Length < 46)
            {
                foreach (Author genre in ListsStaticClass.listAllAuthors)
                {
                    if (text == genre.Name)
                    {
                        check = 1;
                        string error = "Такой автор уже есть!";
                        new ErrorReport(error).ShowDialog(this);
                        break;
                    }
                }
                if (check == 0)
                {
                    var newAdd = new Author
                    {
                        Id = ListsStaticClass.listAllAuthors.LastOrDefault().Id + 1,
                        Name = textForGenre.Text,
                    };
                    Baza.DbContext.Authors.Add(newAdd);
                    Baza.DbContext.SaveChanges();
                    CallBaza();
                    allAuthors.ItemsSource = null;
                    allAuthors.ItemsSource = ListsStaticClass.listAllAuthors.OrderBy(x => x.Id).ToList();
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
        bool hasBooks = ListsStaticClass.listAllBookAuthors.Any(bg => bg.AuthorId == selectId);

        if (hasBooks)
        {
            string errorTwo = "Невозможно удалить автора, у которого есть книга!";
            new ErrorReport(errorTwo).ShowDialog(this);
            return;
        }

        var thisDelete = Baza.DbContext.Authors.FirstOrDefault(x => x.Id == selectId);
        if (thisDelete != null)
        {
            Baza.DbContext.Authors.Remove(thisDelete);
            Baza.DbContext.SaveChanges();
            CallBaza();
            allAuthors.ItemsSource = null;
            allAuthors.ItemsSource = ListsStaticClass.listAllAuthors.OrderBy(x => x.Id).ToList();
        }
    }
}