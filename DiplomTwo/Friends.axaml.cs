using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;
using System.Linq;
using DiplomTwo.Models;
using System.IO;
using System.Collections.Generic;

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
        NextWork();
    }
    public void CallBaza()
    {
        int currentUserId = ListsStaticClass.currentAccount;

        var readers = Baza.DbContext.Readers.ToList();
        var users = Baza.DbContext.Users.ToList();

        // Получаем id пользователей, с кем уже есть любая связь (включая заявки, отклонения, дружбу)
        var relatedUserIds = Baza.DbContext.Friendrelations
            .Where(fr => fr.Fromuserid == currentUserId || fr.Touserid == currentUserId)
            .Select(fr => fr.Fromuserid == currentUserId ? fr.Touserid : fr.Fromuserid)
            .ToHashSet();

        // Исключаем текущего пользователя и тех, с кем уже есть какие-либо связи
        var displayList = (from reader in readers
                           join user in users on reader.UserId equals user.Id
                           where user.Id != currentUserId
                                 && !relatedUserIds.Contains(user.Id)
                                 && user.RoleId != 2 // <-- исключаем администраторов
                           select new ReaderDisplayModel
                           {
                               IdReader = reader.IdReader,
                               Login = user.Login,
                               AvatarUrl = reader.AvatarUrl
                           }).ToList();

        listAllUser.ItemsSource = displayList;
    }

    private void NextWork()
    {
        ListBoxUsers.ItemsSource = null;
        var currentUserId = ListsStaticClass.currentAccount;

        // ID всех пользователей, с кем уже есть связь (друзья, заявки, чёрный список)
        var relatedUsers = Baza.DbContext.Friendrelations
            .Where(fr => fr.Fromuserid == currentUserId || fr.Touserid == currentUserId)
            .Select(fr => fr.Fromuserid == currentUserId ? fr.Touserid : fr.Fromuserid)
            .Distinct()
            .ToList();

        // Все пользователи, с кем нет связи, кроме самого себя
        var potentialFriends = Baza.DbContext.Users
            .Where(u => u.Id != currentUserId && !relatedUsers.Contains(u.Id))
            .ToList();

        List<FriendRecommendation> recommendations = new();

        var currentReader = Baza.DbContext.Readers.FirstOrDefault(r => r.UserId == currentUserId);
        if (currentReader == null) return;

        var currentUserBooks = Baza.DbContext.Personallibraries
            .Where(p => p.ReaderId == currentReader.UserId)
            .Select(p => p.BookId)
            .ToList();

        foreach (var user in potentialFriends)
        {
            var reader = Baza.DbContext.Readers.FirstOrDefault(r => r.UserId == user.Id);
            if (reader == null) continue;

            var userBooks = Baza.DbContext.Personallibraries
                .Where(p => p.ReaderId == reader.IdReader)
                .Select(p => p.BookId)
                .ToList();

            // Совпадения по книгам
            int commonBooks = userBooks.Intersect(currentUserBooks).Count();

            // Пропускаем, если совпадений нет
            if (commonBooks == 0) continue;

            // Подсчёт жанров
            var genreCounts = new Dictionary<string, int>();
            foreach (var bookId in userBooks)
            {
                var genreIds = Baza.DbContext.BookGenres
                    .Where(bg => bg.BookId == bookId)
                    .Select(bg => bg.GenreId)
                    .ToList();

                foreach (var genreId in genreIds)
                {
                    var genreName = Baza.DbContext.Genres.FirstOrDefault(g => g.Id == genreId)?.Name;
                    if (genreName != null)
                    {
                        if (!genreCounts.ContainsKey(genreName))
                            genreCounts[genreName] = 0;
                        genreCounts[genreName]++;
                    }
                }
            }

            var topGenres = genreCounts
                .OrderByDescending(g => g.Value)
                .Take(3)
                .Select(g => g.Key)
                .ToList();

            recommendations.Add(new FriendRecommendation
            {
                UserId = user.Id,
                Login = user.Login,
                ImagePath = reader.AvatarUrl,
                TopGenres = topGenres,
                CommonBooks = commonBooks
            });
        }

        ListBoxUsers.ItemsSource = recommendations;
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
        Close();
    }

    private void AddAskFr(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        int selectedUserId = (int)(sender as Button).Tag;
        int currentUserId = ListsStaticClass.currentAccount;

        // Проверка, есть ли уже отношение между пользователями
        var existingRelation = Baza.DbContext.Friendrelations.FirstOrDefault(fr =>
            (fr.Fromuserid == currentUserId && fr.Touserid == selectedUserId) ||
            (fr.Fromuserid == selectedUserId && fr.Touserid == currentUserId));

        if (existingRelation != null)
        {
            string error = "У вас уже есть заявка!";
            new ErrorReport(error).ShowDialog(this);
            return;
        }

        // Добавляем новую заявку
        var newRequest = new Friendrelation
        {
            Fromuserid = currentUserId,
            Touserid = selectedUserId,
            Statusid = 1, // 1 — заявка отправлена
            Requestdate = DateTime.Now,
        };

        Baza.DbContext.Friendrelations.Add(newRequest);
        Baza.DbContext.SaveChanges();
        NextWork();
    }

    private void AddReq(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        int selectedUserId = (int)(sender as Button).Tag;
        int currentUserId = ListsStaticClass.currentAccount;

        // Проверка, есть ли уже отношение между пользователями
        var existingRelation = Baza.DbContext.Friendrelations.FirstOrDefault(fr =>
            (fr.Fromuserid == currentUserId && fr.Touserid == selectedUserId) ||
            (fr.Fromuserid == selectedUserId && fr.Touserid == currentUserId));

        if (existingRelation != null)
        {
            string error = "У вас уже есть заявка!";
            new ErrorReport(error).ShowDialog(this);
            return;
        }

        // Добавляем новую заявку
        var newRequest = new Friendrelation
        {
            Fromuserid = currentUserId,
            Touserid = selectedUserId,
            Statusid = 1, // 1 — заявка отправлена
            Requestdate = DateTime.Now,
        };

        Baza.DbContext.Friendrelations.Add(newRequest);
        Baza.DbContext.SaveChanges();
        NextWork();
    }
}
public class FriendRecommendation
{
    public int UserId { get; set; }
    public string Login { get; set; }
    public string? ImagePath { get; set; }
    public List<string> TopGenres { get; set; } = new();
    public int CommonBooks { get; set; }
    public Bitmap CoverBitmap
    {
        get
        {
            try
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "AvatarPhoto", ImagePath); return new Bitmap(fullPath);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки изображения: {ex.Message}");
                return null;
            }
        }
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
