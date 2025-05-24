using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;
using System.Linq;
using DiplomTwo.Models;

namespace DiplomTwo;

public partial class ChangingThePriority : Window
{
    private BookPlanDisplay _plan;
    private int _id;
    public ChangingThePriority()
    {
        InitializeComponent();
    }
    public ChangingThePriority(BookPlanDisplay plan)
    {
        InitializeComponent();
        try
        {
            ChangingThePriorityIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
        CallBaza();
        box.ItemsSource = ListsStaticClass.listAllPrioritylevel.ToList();
        _plan = plan; 
        _id = plan.BookId;
        box.SelectedIndex = _plan.PriorityId;

        foreach (Book book in ListsStaticClass.listAllBooks)
        {
            if(book.Id == _plan.BookId)
            {
                imageThis.Source = book.CoverBitmap;
            }
        }

    }

    private void ComboBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        int select = (sender as ComboBox).SelectedIndex;
        select = select + 1;
        var selectBook = Baza.DbContext.Bookplans.FirstOrDefault(x => x.BookId == _id && x.ReaderId == ListsStaticClass.currentAccount);
        if (selectBook != null)
        {        
            selectBook.PriorityId = select;
            new BookPlan().Show();
            Close();
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