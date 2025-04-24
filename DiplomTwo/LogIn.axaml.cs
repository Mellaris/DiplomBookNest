using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Linq;
using DiplomTwo.Models;
using System;
using Avalonia.Media.Imaging;

namespace DiplomTwo;

public partial class LogIn : Window
{
    private int CheckForMassage = 0;
    public LogIn()
    {
        InitializeComponent();
        try
        {
            LogInIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
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

    private void LogInAccount(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(loginCheck.Text) && !string.IsNullOrEmpty(passwordCheck.Text))
        {
            foreach (User a in ListsStaticClass.listAllUsers)
            {
                if (a.Login == loginCheck.Text && a.Passwords == passwordCheck.Text)
                {
                    CheckForMassage = 1;

                    ListsStaticClass.currentAccount = a.Id;
                    var code = GenerateVerificationCode();

                    var verification = new VerificationCode
                    {
                        UserId = a.Id,
                        Code = code,
                        CreatedAt = DateTime.Now,
                    };
                    Baza.DbContext.VerificationCodes.Add(verification);
                    Baza.DbContext.SaveChanges();

                    EmailSender.SendVerificationCode(a.Email, code);

                    new CodeCheckWindow(a.Id, this).ShowDialog(this);
                    break;
                }
            }
        }
        if(CheckForMassage == 0)
        {
            errorMessage.IsVisible = true;
        }
    }
    public static string GenerateVerificationCode()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }
    private void RegistrNewAcc(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Registration().Show();
        Close();
    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }
}