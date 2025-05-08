using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Linq;
using DiplomTwo.Models;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;

namespace DiplomTwo;

public partial class WindowForEditProfile : Window
{
    int helpCheckTwo = 0;
    private string selectedImageFileName = null;
    public WindowForEditProfile()
    {
        InitializeComponent();
        try
        {
            WindowForEditProfileIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
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
    private async void NewEditAvatar(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        helpCheckTwo = 1;
        var dialog = new OpenFileDialog
        {
            AllowMultiple = false,
            Filters = new List<FileDialogFilter>
        {
            new FileDialogFilter { Name = "Images", Extensions = { "png", "jpg", "jpeg", "bmp" } }
        }
        };

        var result = await dialog.ShowAsync(this);

        if (result != null && result.Length > 0)
        {
            var sourcePath = result[0];
            var extension = Path.GetExtension(sourcePath);
            var fileName = $"bookcover_{Guid.NewGuid()}{extension}";

            var destDir = Path.Combine(AppContext.BaseDirectory, "AvatarPhoto");
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);

            var destPath = Path.Combine(destDir, fileName);
            File.Copy(sourcePath, destPath, true);

            selectedImageFileName = fileName;

            using (var stream = File.OpenRead(destPath))
            {
                var bitmap = new Avalonia.Media.Imaging.Bitmap(stream);
                editAvatar.Source = bitmap;
            }
        }
    }
    private void EditAndBack(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(newDescription.Text) && !string.IsNullOrEmpty(newLogin.Text))
        {
            if (newLogin.Text.Count() < 51)
            {
                if (newDescription.Text.Count() < 201)
                {
                    bool loginExists = ListsStaticClass.listAllUsers
                        .Any(u => u.Login == newLogin.Text && u.Id != ListsStaticClass.currentAccount);

                    if (loginExists)
                    {
                        string errorMassege = "Этот логин уже существует";
                        new ErrorReport(errorMassege).ShowDialog(this);
                        return;
                    }

                    var user = Baza.DbContext.Users.FirstOrDefault(u => u.Id == ListsStaticClass.currentAccount);
                    if (user != null)
                    {
                        user.Login = newLogin.Text;

                        
                    }

                    var reader = Baza.DbContext.Readers.FirstOrDefault(u => u.IdReader == ListsStaticClass.currentAccount);
                    if (reader != null)
                    {
                        reader.ProfileDescription = newDescription.Text;
                        if (!string.IsNullOrEmpty(selectedImageFileName) && reader.AvatarUrl != selectedImageFileName)
                        {
                            reader.AvatarUrl = selectedImageFileName;
                        }
                    }

                    Baza.DbContext.SaveChanges();
                    CallBaza();
                    this.Close();
                }
                else
                {
                    string errorMassege = "Описание должно быть меньше 200 символов";
                    new ErrorReport(errorMassege).ShowDialog(this);
                }
            }
            else
            {
                string errorMassege = "Логин должен быть меньше 50 символов";
                new ErrorReport(errorMassege).ShowDialog(this);
            }
        }
        else
        {
            string errorMassege = "Заполните поля";
            new ErrorReport(errorMassege).ShowDialog(this);
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