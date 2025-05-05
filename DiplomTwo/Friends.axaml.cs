using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;
using System.Linq;
using DiplomTwo.Models;
using System.IO;

namespace DiplomTwo;

public partial class Friends : Window
{
    public Friends()
    {
        InitializeComponent();
        try
        {
            FriendsIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
        CallBaza();
    }
    public void CallBaza()
    {
        var readers = Baza.DbContext.Readers.ToList();
        var users = Baza.DbContext.Users.ToList();

        var displayList = (from reader in readers
                           join user in users on reader.UserId equals user.Id
                           select new ReaderDisplayModel
                           {
                               IdReader = reader.IdReader,
                               Login = user.Login,
                               AvatarUrl = reader.AvatarUrl
                           }).ToList();

        listAllUser.ItemsSource = displayList;
    }

    private void Home(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }

    private void AllBook(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Books().Show();
        Close();
    }

    private void MyAccount(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new personalAccount().Show();
        Show();
    }
}
public class ReaderDisplayModel
{
    public int IdReader { get; set; }
    public string? Login { get; set; }
    public string? AvatarUrl { get; set; }
    public Bitmap CoverBitmap
    {
        get
        {
            try
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "AvatarPhoto", AvatarUrl); return new Bitmap(fullPath);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки изображения: {ex.Message}");
                return null;
            }
        }
    }
}