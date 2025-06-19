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
    private List<Book> booksForFinal= new List<Book>();
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
        int currentUser = ListsStaticClass.currentAccount; // текущий пользователь

        // Фильтрация списка книг
        booksForGenre = ListsStaticClass.listAllBooks.Where(book =>
        {
            // Проверка на авторскую книгу
            if (book.IsAuthorBook)
            {
                // Получаем связанные с книгой записи из электронной информации
                var bookInfo = Baza.DbContext.ElectronicBooksInfos
                    .FirstOrDefault(info => info.BookId == book.Id);

                if (bookInfo == null)
                    return false;

                // Проверка по статусу
                if (bookInfo.StatusId == 2 || bookInfo.StatusId == 3)
                {
                    // Если access_level_id = 1, просто показываем книгу
                    if (bookInfo.AccessLevelId == 1)
                    {
                        return true;
                    }

                    // Если access_level_id = 2, проверяем, является ли текущий пользователь автором
                    if (bookInfo.AccessLevelId == 2)
                    {
                        var bookAuthor = Baza.DbContext.Bookauthors
                            .FirstOrDefault(ba => ba.BookId == book.Id && ba.AppAuthorId == currentUser);

                        return bookAuthor != null;
                    }

                    // Если access_level_id = 3, проверяем, есть ли у пользователя в друзьях автор книги
                    if (bookInfo.AccessLevelId == 3)
                    {
                        var bookAuthorId = Baza.DbContext.Bookauthors
                            .Where(ba => ba.BookId == book.Id)
                            .Select(ba => ba.AppAuthorId)
                            .FirstOrDefault();

                        if (bookAuthorId == 0)
                            return false;

                        var isFriend = Baza.DbContext.Friendrelations
                            .Any(f => f.Fromuserid == currentUser && f.Touserid == bookAuthorId && f.Statusid == 2 || f.Touserid == currentUser && f.Fromuserid == bookAuthorId && f.Statusid == 2);

                        return isFriend;
                    }
                }

                return false;
            }

            // Если книга не авторская, просто выводим её
            return true;
        }).ToList();

        allBooks.ItemsSource = booksForGenre;
    }

    private void DisplayForAllFiltr()
    {
        booksForFinal.Clear();
        allBooks.ItemsSource = booksForFinal.ToList();
        booksForFinal = booksForGenre.ToList();

        // Фильтрация по жанру
        if (selectedGenreId > 0)
        {
            booksForFinal = booksForFinal
            .Where(book => book.BookGenres.Any(g => g.GenreId == selectedGenreId))
            .ToList();
        }

        // Фильтрация по новинкам
        if (CheckBoxIsNew.IsChecked == true)
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            booksForFinal = booksForFinal
            .Where(book => book.Dateadd.HasValue &&
            book.Dateadd.Value.Month == currentMonth &&
            book.Dateadd.Value.Year == currentYear)
            .ToList();
        }

        // Фильтрация по авторским книгам
        if (CheckBoxIsAuthorBook.IsChecked == true)
        {
            booksForFinal = booksForFinal
            .Where(book => book.IsAuthorBook == true)
            .ToList();
        }

        // Поиск по названию
        if (!string.IsNullOrEmpty(textForSearch))
        {
            booksForFinal = booksForFinal
            .Where(book => book.Title.ToLower().Contains(textForSearch.ToLower()))
            .ToList();
        }

        // Сортировка по рейтингу
        if (selectRatingId == 0)
        {
            booksForFinal = booksForFinal.OrderBy(book => book.Rating).ToList();
        }
        else if (selectRatingId == 1)
        {
            booksForFinal = booksForFinal.OrderByDescending(book => book.Rating).ToList();
        }

        allBooks.ItemsSource = booksForFinal.ToList();
    }
    private void CheckBoxIsNew_Checked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        DisplayForAllFiltr();
    }
    private void CheckBoxIsNew_Unchecked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        DisplayForAllFiltr();
    }
    private void CheckBoxIsAuthorBook_Checked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        DisplayForAllFiltr();
    }
    private void CheckBoxIsAuthorBook_Unchecked(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        DisplayForAllFiltr();
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

        ListsStaticClass.listAllElectronicBooksInfo.Clear();
        ListsStaticClass.listAllElectronicBooksInfo = Baza.DbContext.ElectronicBooksInfos.ToList();
    }

    private void TextBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        string text = (sender as TextBox).Text;
        textForSearch = text;
        DisplayForAllFiltr();
    }

    private void Home(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }

    private void MeAccount(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new personalAccount().Show();
        Close();
    }
    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (ListsStaticClass.currentAccount != -1)
        {
            new Friends().Show();
            Close();
        }

    }
}