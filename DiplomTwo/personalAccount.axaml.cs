using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Linq;
using DiplomTwo.Models;
using System;
using Avalonia.Media.Imaging;
using System.Collections.Generic;

namespace DiplomTwo;

public partial class personalAccount : Window
{
    private List<Personallibrary> personallibrarySort = new List<Personallibrary>();
    private List<Achievement> achievementSort = new List<Achievement>();
    public personalAccount()
    {
        InitializeComponent();
        try
        {
            personalAccountIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
        CallBaza();
        foreach(User user in ListsStaticClass.listAllUsers)
        {
            if(user.Id == ListsStaticClass.currentAccount)
            {
                readerLogin.Text = user.Login;
                foreach(Reader reader in ListsStaticClass.listAllReaders)
                {
                    if(reader.IdReader == ListsStaticClass.currentAccount)
                    {
                        profileText.Text = reader.ProfileDescription;
                        imageForAvatar.Source = reader.CoverBitmap;
                        break;
                    }
                }
                break;
            }
        }
        int currentYear = DateTime.Now.Year;
        int booksCount = ListsStaticClass.listAllPersonallLibrary
            .Count(pl => pl.ReaderId == ListsStaticClass.currentAccount &&
                        pl.DateAdd.HasValue &&
                        pl.DateAdd.Value.Year == currentYear);
        sumThisYear.Text = booksCount.ToString();
        SortLoveBook();
        int currentUserId = ListsStaticClass.currentAccount;

        var achievementSort = ListsStaticClass.listAllUserachievement
            .Where(ua => ua.ReaderId == currentUserId)
            .Select(ua => ListsStaticClass.listAllAchievement
                .FirstOrDefault(a => a.Id == ua.AchievementId))
            .Where(a => a != null)
            .ToList();

        listWithAch.ItemsSource = achievementSort.ToList();


        // Получаем все книги пользователя из персональной библиотеки
        var userLibraryBooks = ListsStaticClass.listAllPersonallLibrary
            .Where(pl => pl.ReaderId == ListsStaticClass.currentAccount)
            .Select(pl => pl.BookId)
            .Distinct()
            .ToList();

        // Получаем все жанры этих книг (многие ко многим)
        var genresCount = new Dictionary<int, int>(); // id жанра -> сколько раз встречается

        foreach (var bookId in userLibraryBooks)
        {
            var genreIds = ListsStaticClass.listAllBookGenres
                .Where(bg => bg.BookId == bookId)
                .Select(bg => bg.GenreId);

            foreach (var genreId in genreIds)
            {
                if (genresCount.ContainsKey(genreId))
                    genresCount[genreId]++;
                else
                    genresCount[genreId] = 1;
            }
        }

        // Находим топ-3 жанра
        var topGenres = genresCount
            .OrderByDescending(g => g.Value)
            .Take(3)
            .ToList();

        // Сохраняем названия и количества в переменные
        string? topGenreName1 = null, topGenreName2 = null, topGenreName3 = null;
        int topGenreCount1 = 0, topGenreCount2 = 0, topGenreCount3 = 0;

        for (int i = 0; i < topGenres.Count; i++)
        {
            var genreId = topGenres[i].Key;
            var count = topGenres[i].Value;
            var genreName = ListsStaticClass.listAllGenres.FirstOrDefault(g => g.Id == genreId)?.Name;

            if (i == 0) { topGenreName1 = genreName; topGenreCount1 = count; }
            else if (i == 1) { topGenreName2 = genreName; topGenreCount2 = count; }
            else if (i == 2) { topGenreName3 = genreName; topGenreCount3 = count; }
        }
        countOne.Text = topGenreCount2.ToString();
        nameOne.Text = topGenreName2;
        countTwo.Text = topGenreCount1.ToString();
        nameTwo.Text = topGenreName1;
        countThree.Text = topGenreCount3.ToString();
        nameThree.Text = topGenreName3;

        LoadUserStats();
    }
    private void LoadUserStats()
    {
        int userId = ListsStaticClass.currentAccount;

        // Количество книг
        int bookCount = ListsStaticClass.listAllPersonallLibrary
     .Count(pl => pl.ReaderId == userId);
        countBookThis.Text = bookCount.ToString();

        // Количество цитат
        int quoteCount = ListsStaticClass.listAllQuote
     .Count(q => q.ReaderId == userId);
        countQThis.Text = quoteCount.ToString();

        // Количество рецензий
        int reviewCount = ListsStaticClass.listAllBookreview
     .Count(r => r.ReaderId == userId);
        countRevThis.Text = reviewCount.ToString();
    }
    private void OpenBookPlan(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new BookPlan().Show();
        Close();
    }
    private void SortLoveBook()
    {
        foreach(Personallibrary personallibrary in ListsStaticClass.listAllPersonallLibrary)
        {
            if(ListsStaticClass.currentAccount == personallibrary.ReaderId && personallibrary.Rating == 5)
            {
               personallibrarySort.Add(personallibrary);
            }
        }
        // Получаем ID книг
        var lovedBookIds = personallibrarySort.Select(p => p.BookId).ToList();

        // Находим книги по этим ID
        var lovedBooks = ListsStaticClass.listAllBooks
            .Where(book => lovedBookIds.Contains(book.Id))
            .ToList();

        // Устанавливаем в ListBox (предположим, что он называется loveBooksListBox)
        myLoveBook.ItemsSource = lovedBooks.ToList();
    }
    private async void EditProfile(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var editWindow = new WindowForEditProfile();
        await editWindow.ShowDialog(this);

        CallBaza();
        UpdateProfileInfo();
    }
    private void UpdateProfileInfo()
    {
        var user = ListsStaticClass.listAllUsers.FirstOrDefault(u => u.Id == ListsStaticClass.currentAccount);
        if (user != null)
        {
            readerLogin.Text = user.Login;
            var reader = ListsStaticClass.listAllReaders.FirstOrDefault(r => r.IdReader == user.Id);
            if (reader != null)
            {
                profileText.Text = reader.ProfileDescription;
            }
        }
    }
    private void MyAuthorDesk(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        int currentUserId = ListsStaticClass.currentAccount;
        var existingAuthor = Baza.DbContext.Appauthors.FirstOrDefault(u => u.Id == currentUserId);

        if (existingAuthor == null)
        {
            var newAuthor = new Appauthor
            {
                Id = currentUserId,
                ProfileDescription = null,
                WritingGoal = 0,
                AvatarUrl = null,
                WrittenBooksCount = 0,
                RegistrationDate = DateTime.Now,
            };
            Baza.DbContext.Appauthors.Add(newAuthor);
            Baza.DbContext.SaveChanges();
        }

        new AuthorBoard().Show();
        Close();
    }
    private void CallBaza()
    {
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

        ListsStaticClass.listAllUserachievement.Clear();
        ListsStaticClass.listAllUserachievement = Baza.DbContext.Userachievements.Select(userA => new Userachievement
        {
            Id = userA.Id,
            ReaderId = userA.ReaderId,
            AchievementId = userA.AchievementId,
            EarnedAt = userA.EarnedAt,
        }).ToList();

        ListsStaticClass.listAllAchievement.Clear();
        ListsStaticClass.listAllAchievement = Baza.DbContext.Achievements.Select(achievement => new Achievement
        {
            Id = achievement.Id,
            Name = achievement.Name,
            Picturename = achievement.Picturename,
            Description = achievement.Description,
        }).ToList();

        ListsStaticClass.listAllBookGenres.Clear();
        ListsStaticClass.listAllBookGenres = Baza.DbContext.BookGenres.Select(bookGenres => new BookGenre
        {
            Id=bookGenres.Id,
            GenreId = bookGenres.GenreId,
            BookId = bookGenres.BookId,
        }).ToList();

        ListsStaticClass.listAllGenres.Clear();
        ListsStaticClass.listAllGenres = Baza.DbContext.Genres.Select(genres => new Genre
        {
            Id = genres.Id,
            Name = genres.Name,
        }).ToList();
        
        ListsStaticClass.listAllQuote.Clear();
        ListsStaticClass.listAllQuote = Baza.DbContext.Quotes.Select(q => new Quote
        {
            Id = q.Id,
            BookId= q.BookId,
            Text = q.Text,
            ReaderId = q.ReaderId,  
        }).ToList();

        ListsStaticClass.listAllBookreview.Clear();
        ListsStaticClass.listAllBookreview = Baza.DbContext.Bookreviews.Select(br => new Bookreview
        {
            Id = br.Id,
            ReaderId = br.ReaderId,
            BookId = br.BookId,
            ReviewText = br.ReviewText,
            IsHaveRev = br.IsHaveRev,
            CreatedAt = br.CreatedAt,
        }).ToList();
    }
    private void Log(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new LogIn().Show();
        Close();
    }

    private void MyLibraryBooks(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MyBooks().Show();
        Close();
    }
    private void OpenFriends(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MyOwnFriends().Show();
        Close();
    }

    private void Home(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }

    private void OpenB(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Books().Show();
        Close();
    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Friends().Show();
        Close();
    }
}