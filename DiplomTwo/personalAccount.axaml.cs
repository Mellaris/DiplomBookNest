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
        foreach(Userachievement userachievement in ListsStaticClass.listAllUserachievement)
        {
            if(userachievement.Id == ListsStaticClass.currentAccount)
            {
                foreach(Achievement achievement in ListsStaticClass.listAllAchievement)
                {
                    if(userachievement.AchievementId == achievement.Id)
                    {
                        achievementSort.Add(achievement);
                    }
                }
                break;
            }
        }
        listWithAch.ItemsSource = achievementSort.ToList();
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
}