using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using DiplomTwo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiplomTwo;

public partial class BookPlan : Window
{
    private List<BookPlanDisplay> bookDisplayModels = new List<BookPlanDisplay>();
    public BookPlan()
    {
        InitializeComponent();
        try
        {
            BookPlanIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
        CallBaza();
        var planList = ListsStaticClass.listAllBookPlan
            .Where(x => x.ReaderId == ListsStaticClass.currentAccount)
            .Join(ListsStaticClass.listAllBooks,
            plan => plan.BookId,
            book => book.Id,
        (plan, book) => new BookPlanDisplay
        {
            Id = plan.Id,
            BookId = book.Id,
            BookTitle = book.Title,
            PriorityId = plan.PriorityId,
            PriorityColor = plan.PriorityId switch
            {
                1 => Brushes.Red,
                2 => Brushes.Orange,
                3 => Brushes.Green,
                4 => Brushes.Gray,
            }
        })
    .OrderBy(x => x.PriorityId) // При необходимости сортировка
    .ToList();

        myPlan.ItemsSource = planList.ToList();
        bookDisplayModels = planList.ToList();
    }
    private void EditGoal(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        textForGoal.IsReadOnly = false;
        textForGoal.IsEnabled = true;
        textForGoal.Focus();
        
    }
    private void TextBox_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (e.Key == Avalonia.Input.Key.Enter)
        {
            if (int.TryParse(textForGoal.Text, out int newGoal))
            {
                var user = Baza.DbContext.Readers.FirstOrDefault(u => u.IdReader == ListsStaticClass.currentAccount);
                if (user != null)
                {              
                    user.YearlyGoal = newGoal;
                    Baza.DbContext.SaveChanges();
                    CallBaza();
                    textForGoal.IsReadOnly = true;
                    textForGoal.IsEnabled = false;
                }
            }
            else
            {
                string error = "Вы можете ввести только цифры!";
                new ErrorReport(error).ShowDialog(this);
            }
        }
    }
    private void EditPrioritet(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        int selectId = (int)(sender as Button).Tag;
        var button = bookDisplayModels.FirstOrDefault(x => x.Id == selectId);
        if(button != null)
        {
           new ChangingThePriority(button).ShowDialog(this);
        }
    }
    private void MyAccount(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new personalAccount().Show();
        Close();
    }

    private void Log(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new LogIn().Show();
        Close();
    }

    private void AllBooks(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Books().Show();
        Close();
    }
    private void CallBaza()
    {
        ListsStaticClass.listAllBooks.Clear();
        ListsStaticClass.listAllBooks = Baza.DbContext.Books.Select(book => new Book
        {
            Id = book.Id,
            Title = book.Title,
            Rating = book.Rating,
            Kolread = book.Kolread,
            Kolplan = book.Kolplan,
            Kolrev = book.Kolrev,
            AgeLimit = book.AgeLimit,
            PageCount = book.PageCount,
            CoverImage = book.CoverImage,
            Synopsis = book.Synopsis,
            Description = book.Description,
            IsAuthorBook = book.IsAuthorBook,
            Dateadd = book.Dateadd,
            SeriesId = book.SeriesId,
            BookGenres = book.BookGenres.ToList(),
        }).ToList();

        ListsStaticClass.listAllPrioritylevel.Clear();
        ListsStaticClass.listAllPrioritylevel = Baza.DbContext.Prioritylevels.Select(plan => new Prioritylevel
        {
            Id = plan.Id,
            Color = plan.Color,
            Name = plan.Name,
        }).ToList();

        ListsStaticClass.listAllBookPlan.Clear();
        ListsStaticClass.listAllBookPlan = Baza.DbContext.Bookplans.Select(bookPlan => new Bookplan
        {
            Id = bookPlan.Id,
            BookId = bookPlan.BookId,
            ReaderId = bookPlan.ReaderId,
            PriorityId = bookPlan.PriorityId,
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
}
public class BookPlanDisplay
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string BookTitle { get; set; }
    public int PriorityId {  get; set; }
    public IBrush PriorityColor { get; set; }
}