using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DiplomTwo.Models;
using System;
using System.Linq;

namespace DiplomTwo;

public partial class Registration : Window
{
    private int vereficationCodeTwo;
    private int gender = 3;
    public Registration()
    {
        InitializeComponent();
        CallBaza();
    }
    private void NewUser(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(name.Text) || string.IsNullOrEmpty(lastName.Text) || string.IsNullOrEmpty(login.Text) ||
            string.IsNullOrEmpty(email.Text) || string.IsNullOrEmpty(password.Text))
        {
            errorMessage.Text = "Все поля должны быть заполнены";
            errorVisible.IsVisible = true;
            return;
        }

        if (!IsValidEmail(email.Text))
        {
            errorMessage.Text = "Некорректный адрес электронной почты";
            errorVisible.IsVisible = true;
            return;
        }

        if (IsEmailExists(email.Text))
        {
            errorMessage.Text = "Этот адрес электронной почты уже занят";
            errorVisible.IsVisible = true;
            return;
        }

        if (IsLoginExists(login.Text))
        {
            errorMessage.Text = "Этот логин уже занят";
            errorVisible.IsVisible = true;
            return;
        }

        if (!(manButton.IsChecked.HasValue && manButton.IsChecked.Value) &&
           !(womanButton.IsChecked.HasValue && womanButton.IsChecked.Value))
        {
            errorMessage.Text = "Выберите пол.";
            errorMessage.IsVisible = true;
            return;
        }
        else
        {
            var vereficationCode = GenerateVerificationCode();
            vereficationCodeTwo = Convert.ToInt32(vereficationCode);
            string emailForCode = email.Text;
            EmailSender.SendVerificationCode(emailForCode, vereficationCode);
            checkCodeVisible.IsVisible = true;
            errorMessage.IsVisible = false;

            name.IsEnabled = false;
            lastName.IsEnabled = false;
            login.IsEnabled = false;
            email.IsEnabled = false;
            password.IsEnabled = false;
        }
        if (manButton.IsChecked == true)
        {
            gender = 2;
        }
        else if (womanButton.IsChecked == true)
        {
            gender = 1;
        }
        

    }
    private void TextBox_TextChanged_1(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(checkCode.Text))
        {
            if (checkCode.Text == vereficationCodeTwo.ToString())
            {
                ListsStaticClass.listAllUsers = ListsStaticClass.listAllUsers.OrderBy(user => user.Id).ToList();
                var user = new User
                {
                    Id = ListsStaticClass.listAllUsers.LastOrDefault().Id + 1,
                    Username = name.Text,
                    Userlastname = lastName.Text,
                    Login = login.Text,
                    Email = email.Text,
                    Passwords = password.Text,
                    RoleId = 1,
                    GenderId = gender,
                    RegistrationDate = DateTime.Now,
                };
                Baza.DbContext.Users.Add(user);
                Baza.DbContext.SaveChanges();

                int newUserId = user.Id;
                ListsStaticClass.currentAccount = newUserId;
                CallBaza();

                var reader = new Reader
                {
                   IdReader = newUserId,
                   UserId = newUserId,
                };
                Baza.DbContext.Readers.Add(reader);
                Baza.DbContext.SaveChanges();

                new personalAccount().Show();
                Close();

            }
        }
    }
    private void CallBaza()
    {
        ListsStaticClass.listAllUsers.Clear();
        ListsStaticClass.listAllUsers = Baza.DbContext.Users.Select(user => new User
        {
            Id = user.Id,
            Username = user.Username,
            Userlastname = user.Userlastname,
            Login = user.Login,
            Email = user.Email,
            Passwords = user.Passwords,
            RoleId = user.RoleId,
            GenderId = user.GenderId,
            RegistrationDate = user.RegistrationDate,
        }).ToList();
    }
    public static string GenerateVerificationCode()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }
    private bool IsValidEmail(string email)
    {
        var emailRegex = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        return emailRegex.IsMatch(email);
    }
    private bool IsEmailExists(string email)
    {
        return Baza.DbContext.Users.Any(user => user.Email == email);
    }
    private bool IsLoginExists(string login)
    {
        return Baza.DbContext.Users.Any(user => user.Login == login);
    }

    private void LogInAcc(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new LogIn().Show();
        Close();
    }


}