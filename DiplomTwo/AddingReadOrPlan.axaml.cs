using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using DiplomTwo.Models;
using System;
using System.Linq;

namespace DiplomTwo;

public partial class AddingReadOrPlan : Window
{
    private int idThis;
    private int check = 0;
    private int select = 1;
    public AddingReadOrPlan()
    {
        InitializeComponent();
    }
    public AddingReadOrPlan(int id)
    {
        InitializeComponent();
        try
        {
            AddingReadOrPlanIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
        idThis = id;
        CallBaza();
    }
    private void AddInPlan(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ListsStaticClass.listAllBookPlan = ListsStaticClass.listAllBookPlan.OrderBy(x => x.Id).ToList();
        var addNewBookPlan = new Bookplan
        {
            Id = ListsStaticClass.listAllBookPlan.LastOrDefault().Id + 1,
            BookId = idThis,
            ReaderId = ListsStaticClass.currentAccount,
            PriorityId = select
        };
        Baza.DbContext.Bookplans.Add(addNewBookPlan);
        Baza.DbContext.SaveChanges();
        Close(this);

    }
    private void ComboBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        select = (sender as ComboBox).SelectedIndex;
        select = select + 1;
    }
    private void AddPlan(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        foreach(Bookplan book in ListsStaticClass.listAllBookPlan)
        {
            if(ListsStaticClass.currentAccount == book.ReaderId && book.BookId == idThis)
            {
                string error = "¬ы уже добавл€ли эту книгу в книжный план!";
                check = 1;
                new ErrorReport(error).ShowDialog(this);
                break;
            }
        }
        if(check == 0)
        {
            panelVisible.IsVisible = true;
            readPanel.IsVisible = false;

            foreach (Book a in ListsStaticClass.listAllBooks)
            {
                if (a.Id == idThis)
                {
                    imageForPlan.Source = a.CoverBitmap;
                }
            }
            boxForPrior.ItemsSource = ListsStaticClass.listAllPrioritylevel.ToList();
            boxForPrior.SelectedIndex = 0;
        }
        
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
    }

    
}