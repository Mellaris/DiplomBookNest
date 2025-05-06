using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using DiplomTwo.Models;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DiplomTwo;

public partial class MyOwnFriends : Window
{
    public MyOwnFriends()
    {
        InitializeComponent();
        try
        {
            MyOwnFriendsIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
        Help();
        MyFr();

    }
    private void MyFr()
    {
        int currentUserId = ListsStaticClass.currentAccount;

        // Сначала загружаем все подтверждённые заявки (статус 2), где текущий пользователь — одна из сторон
        var confirmedFriendRelations = Baza.DbContext.Friendrelations
            .Where(fr => fr.Statusid == 2 &&
                        (fr.Fromuserid == currentUserId || fr.Touserid == currentUserId))
            .ToList();

        // Затем формируем список друзей
        var friends = new List<User>();

        foreach (var relation in confirmedFriendRelations)
        {
            int friendId = relation.Fromuserid == currentUserId
                ? relation.Touserid
                : relation.Fromuserid;

            var friend = Baza.DbContext.Users.FirstOrDefault(u => u.Id == friendId);
            if (friend != null)
            {
                friends.Add(friend);
            }
        }
        allMyFr.ItemsSource = null;
        // Устанавливаем источник данных для ListBox
        allMyFr.ItemsSource = friends.ToList();
    }
    private void OpenBlackList(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        int currentUserId = ListsStaticClass.currentAccount;

        var rejectedRelations = Baza.DbContext.Friendrelations
            .Where(fr => fr.Touserid == currentUserId && fr.Statusid == 3)
            .ToList();

        var users = Baza.DbContext.Users.ToList();
        var readers = Baza.DbContext.Readers.ToList();

        var displayList = (from request in rejectedRelations
                           join user in users on request.Fromuserid equals user.Id
                           join reader in readers on user.Id equals reader.UserId
                           select new FriendRequestDisplay
                           {
                               Id = user.Id,
                               Login = user.Login,
                               Description = reader.ProfileDescription,
                               AvatarUrl = reader.AvatarUrl
                           }).ToList();
        myApplications.ItemsSource = null;
        myApplications.ItemsSource = displayList.ToList();
    }

    private void Help()
    {
        int currentUserId = ListsStaticClass.currentAccount;

        // Получаем все входящие заявки со статусом 1
        var incomingRequests = Baza.DbContext.Friendrelations
            .Where(fr => fr.Touserid == currentUserId && fr.Statusid == 1)
            .ToList();

        var users = Baza.DbContext.Users.ToList();
        var readers = Baza.DbContext.Readers.ToList();

        var displayList = (from request in incomingRequests
                           join user in users on request.Fromuserid equals user.Id
                           join reader in readers on user.Id equals reader.UserId
                           select new FriendRequestDisplay
                           {
                               Id = user.Id,
                               Login = user.Login,
                               Description = reader.ProfileDescription,
                               AvatarUrl = reader.AvatarUrl
                           }).ToList();
        myApplications.ItemsSource = null;
        myApplications.ItemsSource = displayList.ToList();
    }
    private void RejectFriendRequest(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        int fromUserId = (int)(sender as Button).Tag;
        int currentUserId = ListsStaticClass.currentAccount;

        // Ищем заявку
        var relation = Baza.DbContext.Friendrelations
            .FirstOrDefault(fr => fr.Fromuserid == fromUserId &&
                                  fr.Touserid == currentUserId &&
                                  fr.Statusid == 1);

        if (relation != null)
        {
            relation.Statusid = 3; // Статус "Отклонён"
            Baza.DbContext.SaveChanges();
            Help();
        }
    }
    private void AcceptFriendRequest(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        int fromUserId = (int)(sender as Button).Tag;
        int currentUserId = ListsStaticClass.currentAccount;

        // Ищем заявку от этого пользователя к текущему
        var relation = Baza.DbContext.Friendrelations
            .FirstOrDefault(fr => fr.Fromuserid == fromUserId &&
                                  fr.Touserid == currentUserId &&
                                  fr.Statusid == 1 || fr.Statusid == 3);

        if (relation != null)
        {
            relation.Statusid = 2; // Статус "Приняты"
            Baza.DbContext.SaveChanges();
            MyFr();
            Help();
        }
    }
    private void LoadIncomingFriendRequests(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Help();
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

    private void My(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new personalAccount().Show();
        Close();
    }

}
public class FriendRequestDisplay
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Description { get; set; }
    public string AvatarUrl { get; set; }
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