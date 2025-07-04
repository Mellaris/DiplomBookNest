using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using DiplomTwo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Tmds.DBus.Protocol;

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
        foreach(Models.Reader readerr in ListsStaticClass.listAllReaders)
        {
            if(readerr.UserId == ListsStaticClass.currentAccount)
            {
                textForGoal.Text = readerr.YearlyGoal.ToString();
                goalTwp.Text = readerr.YearlyGoal.ToString();
                break;
            }
        }
        var openCount = ListsStaticClass.listAllPersonallLibrary.Where(x => x.ReaderId == ListsStaticClass.currentAccount).ToList();
        currentProgres.Text = openCount.Count.ToString();
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
    .OrderBy(x => x.PriorityId) // ��� ������������� ����������
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
                    textForGoal.Text = user.YearlyGoal.ToString();
                    goalTwp.Text = user.YearlyGoal.ToString();
                    Baza.DbContext.SaveChanges();
                    CallBaza();
                    textForGoal.IsReadOnly = true;
                    textForGoal.IsEnabled = false;
                }
            }
            else
            {
                string error = "�� ������ ������ ������ �����!";
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
            try
            {
                new ChangingThePriority(button).Show();
                Close();
            }
            catch { }
        }
    }
    private void MyAccount(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new personalAccount().Show();
        Close();
    }

    private void Log(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new personalAccount().Show();
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
        ListsStaticClass.listAllReaders = Baza.DbContext.Readers.ToList();

      
    }

    private void OpenAndAdd(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        int idBookCh = (int)(sender as Button).Tag;
        new WritingReview(idBookCh).Show();
        Close();
    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Friends().Show();
        Close();
    }

    private void DeletePrior(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        int idToDelete = (int)(sender as Button).Tag;

        // ������� ������ ����� � ����
        var planToRemove = Baza.DbContext.Bookplans
            .FirstOrDefault(p => p.BookId == idToDelete && p.ReaderId == ListsStaticClass.currentAccount);

        if (planToRemove != null)
        {
            // ������� �� ����
            Baza.DbContext.Bookplans.Remove(planToRemove);
            Baza.DbContext.SaveChanges();

            // ��������� ������ � ������� � UI
            CallBaza();

            // ������� �� ����������� ������ (bookDisplayModels + ItemsSource)
            var displayToRemove = bookDisplayModels.FirstOrDefault(b => b.BookId == idToDelete);
            if (displayToRemove != null)
            {
                bookDisplayModels.Remove(displayToRemove);
                myPlan.ItemsSource = null;
                myPlan.ItemsSource = bookDisplayModels;
            }
        }
        else
        {
            string error = "�� ������� ����� ����� ��� ��������.";
            new ErrorReport(error).ShowDialog(this);
        }
    }

}
