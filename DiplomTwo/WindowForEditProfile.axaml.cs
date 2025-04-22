using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Linq;
using DiplomTwo.Models;

namespace DiplomTwo;

public partial class WindowForEditProfile : Window
{
    public WindowForEditProfile()
    {
        InitializeComponent();
        CallBaza();
        foreach(User user in ListsStaticClass.listAllUsers)
        {
            if(user.Id == ListsStaticClass.currentAccount)
            {
                newLogin.IsEnabled = true;
                newLogin.Text = user.Login;
                newLogin.IsEnabled = false;
                foreach (Reader reader in ListsStaticClass.listAllReaders)
                {
                    if (reader.IdReader == ListsStaticClass.currentAccount)
                    {
                        newDescription.IsEnabled = true;
                        newDescription.Text = reader.ProfileDescription;
                        newDescription.IsEnabled = false;
                        break;
                    }
                }
                break;
            }
        }
    }
    private void EditAndBack(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if(!string.IsNullOrEmpty(newDescription.Text) && !string.IsNullOrEmpty(newLogin.Text))
        {
            if(newLogin.Text.Count() < 51)
            {
                if (newDescription.Text.Count() < 201)
                {
                    errorMassege.IsVisible = false;
                    bool loginExists = ListsStaticClass.listAllUsers
                        .Any(u => u.Login == newLogin.Text && u.Id != ListsStaticClass.currentAccount);
                    if (loginExists)
                    {
                        errorMassege.IsVisible = true;
                        errorMassege.Text = "Этот логин уже существует";
                        return;
                    }

                    var user = Baza.DbContext.Users.FirstOrDefault(u => u.Id == ListsStaticClass.currentAccount);
                    if (user != null)
                    {
                        errorMassege.IsVisible = false;
                        user.Login = newLogin.Text;
                    }

                    var reader = Baza.DbContext.Readers.FirstOrDefault(u => u.IdReader == ListsStaticClass.currentAccount);
                    if (reader != null)
                    {
                        reader.ProfileDescription = newDescription.Text;
                    }
                    Baza.DbContext.SaveChanges();
                    CallBaza();
                    this.Close();
                }
                else
                {
                    errorMassege.IsVisible = true;
                    errorMassege.Text = "Описание должно быть меньше 200 символов";
                }
            }
            else
            {
                errorMassege.IsVisible = true;
                errorMassege.Text = "Логин должен быть меньше 50 символов";
            }
        }
        else
        {
            errorMassege.IsVisible = true;
            errorMassege.Text = "Заполните поля";
        }
    }
    private void OpenEditLogin(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        newLogin.IsEnabled = true;
    }
    private void OpenEditDescription(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        newDescription.IsEnabled = true;
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
    }
    private void Back(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close();
    }
}