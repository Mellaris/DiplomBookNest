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
        int currentUserId = ListsStaticClass.currentAccount;

        var readers = Baza.DbContext.Readers.ToList();
        var users = Baza.DbContext.Users.ToList();

        // �������� id �������������, � ��� ��� ���� ����� ����� (������� ������, ����������, ������)
        var relatedUserIds = Baza.DbContext.Friendrelations
            .Where(fr => fr.Fromuserid == currentUserId || fr.Touserid == currentUserId)
            .Select(fr => fr.Fromuserid == currentUserId ? fr.Touserid : fr.Fromuserid)
            .ToHashSet();

        // ��������� �������� ������������ � ���, � ��� ��� ���� �����-���� �����
        var displayList = (from reader in readers
                           join user in users on reader.UserId equals user.Id
                           where user.Id != currentUserId && !relatedUserIds.Contains(user.Id)
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
        Close();
    }

    private void AddAskFr(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        int selectedUserId = (int)(sender as Button).Tag;
        int currentUserId = ListsStaticClass.currentAccount;

        // ��������, ���� �� ��� ��������� ����� ��������������
        var existingRelation = Baza.DbContext.Friendrelations.FirstOrDefault(fr =>
            (fr.Fromuserid == currentUserId && fr.Touserid == selectedUserId) ||
            (fr.Fromuserid == selectedUserId && fr.Touserid == currentUserId));

        if (existingRelation != null)
        {
            string error = "� ��� ��� ���� ������!";
            new ErrorReport(error).ShowDialog(this);
            return;
        }

        // ��������� ����� ������
        var newRequest = new Friendrelation
        {
            Fromuserid = currentUserId,
            Touserid = selectedUserId,
            Statusid = 1, // 1 � ������ ����������
            Requestdate = DateTime.Now,
        };

        Baza.DbContext.Friendrelations.Add(newRequest);
        Baza.DbContext.SaveChanges();
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
                Console.WriteLine($"������ �������� �����������: {ex.Message}");
                return null;
            }
        }
    }
}