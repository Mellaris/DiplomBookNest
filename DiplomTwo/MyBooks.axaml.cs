using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using DiplomTwo.Models;
using System;
using System.Linq;

namespace DiplomTwo;

public partial class MyBooks : Window
{
    private string? currentSearchText = null;
    private int? currentSortIndex = null;

    public MyBooks()
    {
        InitializeComponent();
        try
        {
            MyBooksIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
        CallBaza();
        UpdateDisplayedBooks();
    }
    private void UpdateDisplayedBooks(string? searchText = null, int? sortIndex = null)
    {
        currentSearchText = searchText ?? currentSearchText;
        currentSortIndex = sortIndex ?? currentSortIndex;

        int currentUserId = ListsStaticClass.currentAccount;

        var userBooks = ListsStaticClass.listAllPersonallLibrary
            .Where(lib => lib.ReaderId == currentUserId)
            .Select(lib =>
            {
                var book = ListsStaticClass.listAllBooks.FirstOrDefault(b => b.Id == lib.BookId);
                if (book == null) return null;

                var bookAuthor = ListsStaticClass.listAllBookAuthors
                    .FirstOrDefault(ba => ba.BookId == book.Id);

                string authorName = "Автор неизвестен";

                if (book.IsAuthorBook)
                {
                    var appAuthor = ListsStaticClass.listAllAppAuthors
                        .FirstOrDefault(aa => aa.Id == bookAuthor?.AppAuthorId);

                    var user = ListsStaticClass.listAllUsers
                        .FirstOrDefault(u => u.Id == appAuthor?.Id);

                    authorName = user?.Login ?? "Автор неизвестен";
                }
                else
                {
                    var author = ListsStaticClass.listAllAuthors
                        .FirstOrDefault(a => a.Id == bookAuthor?.AuthorId);

                    authorName = author?.Name ?? "Автор неизвестен";
                }

                return new BookDisplayModel
                {
                    BookId = book.Id,
                    Title = book.Title,
                    Synopsis = book.Synopsis,
                    Quote = book.Quote,
                    CoverBitmap = book.CoverBitmap,
                    AuthorName = authorName,
                    Rating = lib.Rating,
                    PlotRating = lib.PlotRating,
                    CharactersRating = lib.CharactersRating,
                    WorldRating = lib.WorldRating,
                    RomanceRating = lib.RomanceRating,
                    HumorRating = lib.HumorRating,
                    MeaningRating = lib.MeaningRating
                };
            })
            .Where(x => x != null)
            .ToList();

        // Поиск по названию
        if (!string.IsNullOrWhiteSpace(currentSearchText))
        {
            userBooks = userBooks
                .Where(book => book.Title.Contains(currentSearchText, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // Сортировка по рейтингу
        if (currentSortIndex == 0) // По возрастанию
        {
            userBooks = userBooks.OrderBy(b => b.Rating ?? 0).ToList();
        }
        else if (currentSortIndex == 1) // По убыванию
        {
            userBooks = userBooks.OrderByDescending(b => b.Rating ?? 0).ToList();
        }

        allMyBooks.ItemsSource = userBooks;
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
            Quote = book.Quote,
            BookGenres = book.BookGenres.ToList(),
        }).ToList();

        ListsStaticClass.listAllUsers.Clear();
        ListsStaticClass.listAllUsers = Baza.DbContext.Users.Select(user => new User
        {
            Id = user.Id,
            Username = user.Username,
            Userlastname = user.Userlastname,
            Email = user.Email,
            Passwords = user.Passwords,
            Login = user.Login,
            RoleId = user.RoleId,
            GenderId = user.GenderId,
            RegistrationDate = user.RegistrationDate,
        }).ToList();

        ListsStaticClass.listAllReaders.Clear();
        ListsStaticClass.listAllReaders = Baza.DbContext.Readers.Select(reader => new Reader
        {
            IdReader = reader.IdReader,
            UserId = reader.UserId,
            YearlyGoal = reader.YearlyGoal,
            AvatarUrl = reader.AvatarUrl,
            ProfileDescription = reader.ProfileDescription,
        }).ToList();

        ListsStaticClass.listAllPersonallLibrary.Clear();
        ListsStaticClass.listAllPersonallLibrary = Baza.DbContext.Personallibraries.Select(library => new Personallibrary
        {
            Id = library.Id,
            ReaderId = library.ReaderId,
            BookId = library.BookId,
            Rating = library.Rating,
            PlotRating = library.PlotRating,
            CharactersRating = library.CharactersRating,
            WorldRating = library.WorldRating,
            RomanceRating = library.RomanceRating,
            HumorRating = library.HumorRating,
            MeaningRating = library.MeaningRating,
            DateAdd = library.DateAdd,
            Feedback = library.Feedback,
        }).ToList();

        ListsStaticClass.listAllBookAuthors.Clear();
        ListsStaticClass.listAllBookAuthors = Baza.DbContext.Bookauthors.Select(ba => new Bookauthor
        {
            Id = ba.Id,
            AuthorId = ba.AuthorId,
            AppAuthorId = ba.AppAuthorId,
            BookId = ba.BookId,
        }).ToList();

        ListsStaticClass.listAllAuthors.Clear();
        ListsStaticClass.listAllAuthors = Baza.DbContext.Authors.Select(authors => new Author
        {
            Id = authors.Id,
            Name = authors.Name,
        }).ToList();

        ListsStaticClass.listAllAppAuthors.Clear();
        ListsStaticClass.listAllAppAuthors = Baza.DbContext.Appauthors.Select(appA => new Appauthor
        {
            Id = appA.Id,
            RegistrationDate = appA.RegistrationDate,
        }).ToList();

        ListsStaticClass.listAllQuote.Clear();
        ListsStaticClass.listAllQuote = Baza.DbContext.Quotes.Select(q => new Quote
        {
            Id = q.Id,
            BookId = q.BookId,
            ReaderId = q.ReaderId,
            Text = q.Text,
        }).ToList();

        ListsStaticClass.listAllBookreview.Clear();
        ListsStaticClass.listAllBookreview = Baza.DbContext.Bookreviews.Select(br => new Bookreview 
        { 
            Id = br.Id,
            BookId= br.BookId,
            ReaderId = br.ReaderId,
            ReviewText = br.ReviewText,
        }).ToList();
    }

    private void Home(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }

    private void OpenBooks(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Books().Show();
        Close();
    }

    private void My(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new personalAccount().Show();
        Close();
    }

    private void OpenAllBooks(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Books().Show();
        Close();
    }

    private void OpenRevBook(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        int selectIdBookForOpen = (int)(sender as Button).Tag;
        new WritingReview(selectIdBookForOpen).Show();
        Close();
    }

    private void DeleteThisBook(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // Получаем ID книги из Tag
        int bookIdSelect = (int)(sender as Button).Tag;
        int currentUserId = ListsStaticClass.currentAccount; // Текущий пользователь

        // Удаляем запись из таблицы персональной библиотеки в базе данных
        var personalLibraryEntry = Baza.DbContext.Personallibraries
            .FirstOrDefault(lib => lib.ReaderId == currentUserId && lib.BookId == bookIdSelect);
        if (personalLibraryEntry != null)
        {
            ListsStaticClass.listAllPersonallLibrary.Remove(personalLibraryEntry);
            Baza.DbContext.Personallibraries.Remove(personalLibraryEntry);
        }

        // Удаляем цитаты для этой книги и пользователя
        var quotesToRemove = Baza.DbContext.Quotes
            .Where(q => q.BookId == bookIdSelect && q.ReaderId == currentUserId)
            .ToList();
        foreach (var quote in quotesToRemove)
        {
            Baza.DbContext.Quotes.Remove(quote);
        }

        // Удаляем отзывы для этой книги и пользователя
        var reviewsToRemove = Baza.DbContext.Bookreviews
            .Where(r => r.BookId == bookIdSelect && r.ReaderId == currentUserId)
            .ToList();
        foreach (var review in reviewsToRemove)
        {
            Baza.DbContext.Bookreviews.Remove(review);
        }

        // Пересчитываем рейтинг книги
        var otherUsersRatings = Baza.DbContext.Personallibraries
            .Where(lib => lib.BookId == bookIdSelect && lib.ReaderId != currentUserId) // Игнорируем текущего пользователя
            .ToList();

        if (otherUsersRatings.Any())
        {
            double totalRating = 0;
            int ratingCount = 0;

            // Суммируем все рейтинги книги от других пользователей
            foreach (var lib in otherUsersRatings)
            {
                totalRating += lib.Rating ?? 0.0; // Используем рейтинг из персональной библиотеки
                ratingCount++;
            }

            // Рассчитываем новый рейтинг
            double averageRating = totalRating / ratingCount;

            // Округляем до одной цифры после запятой
            double finalRating = Math.Round(averageRating, 1);

            // Обновляем рейтинг в таблице book
            var bookToUpdate = Baza.DbContext.Books.FirstOrDefault(b => b.Id == bookIdSelect);
            if (bookToUpdate != null)
            {
                bookToUpdate.Rating = finalRating;
            }
        }
        else
        {
            // Если нет других пользователей, можно поставить 0 или оставить без изменений
            var bookToUpdate = Baza.DbContext.Books.FirstOrDefault(b => b.Id == bookIdSelect);
            if (bookToUpdate != null)
            {
                bookToUpdate.Rating = 0;
            }
        }

        // Сохраняем изменения в базе данных
        Baza.DbContext.SaveChanges();

        // Обновляем отображение списка
        UpdateDisplayedBooks(); // Или другой метод для обновления интерфейса
    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Friends().Show();
        Close();
    }
    private void TextBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        var textBox = sender as TextBox;
        string searchText = textBox?.Text ?? "";
        UpdateDisplayedBooks(searchText: searchText);
    }

    private void ComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var comboBox = sender as ComboBox;
        int selectedIndex = comboBox?.SelectedIndex ?? 0;
        UpdateDisplayedBooks(sortIndex: selectedIndex);
    }

}