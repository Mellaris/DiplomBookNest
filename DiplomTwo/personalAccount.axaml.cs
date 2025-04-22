using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Linq;
using DiplomTwo.Models;
using System;

namespace DiplomTwo;

public partial class personalAccount : Window
{
    public personalAccount()
    {
        InitializeComponent();
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
    }

    
}