using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Linq;
using DiplomTwo.Models;

namespace DiplomTwo;

public partial class LogIn : Window
{
    public LogIn()
    {
        InitializeComponent();
        SpisokAllUsers();
    }
    private void SpisokAllUsers()
    {
        ListsStaticClass.listAllUsers.Clear();
        ListsStaticClass.listAllUsers = Baza.DbContext.Users.Select(user => new User
        {
            Id = user.Id,
            Username = user.Username,
            Login = user.Login,
            Email = user.Email,
            Passwords = user.Passwords,
            RegistrationDate = user.RegistrationDate,
            Userlastname = user.Userlastname,
        }).ToList();
    }

    private void RegistrNewAcc(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Registration().Show();
        Close();
    }

    private void LogInAccount(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        //if(!string.IsNullOrEmpty(loginCheck.Text) && !string.IsNullOrEmpty(passwordCheck.Text))
        //{
        //    foreach(User a in ListsStaticClass.listAllUsers)
        //    {
        //        if(a.Login == loginCheck.Text && a.Passwords == passwordCheck.Text)
        //        {
        //            ListsStaticClass.currentAccount = a.Id;
        //            new personalAccount().Show();
        //            Close();
        //        }
        //    }
        //}
        //errorMessage.IsVisible = true;
    }
}