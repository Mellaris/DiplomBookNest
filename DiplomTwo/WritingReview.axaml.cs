using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;
using System.Linq;
using DiplomTwo.Models;

namespace DiplomTwo;

public partial class WritingReview : Window
{
    private int idThisBook;
    private int select;
    private int selectAuthor = -1;
    private int selectAuthorApp = -1;
    public WritingReview()
    {
        InitializeComponent();
    }
    public WritingReview(int id)
    {
        InitializeComponent();
        try
        {
            WritingReviewIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
        idThisBook = id;
        CallBaza();
        // Получаем нужную книгу
        var book = ListsStaticClass.listAllBooks.FirstOrDefault(b => b.Id == idThisBook);
        if (book is null) return;

        // Устанавливаем свойства книги
        imageForBook.Source = book.CoverBitmap;
        contentBook.Text = book.Description;
        nameBook.Text = book.Title;
        age.Text = book.AgeLimit.ToString();
        page.Text = book.PageCount.ToString();

        // Находим соответствующего автора
        var bookAuthor = ListsStaticClass.listAllBookAuthors
            .FirstOrDefault(ba => ba.BookId == idThisBook);

        if (bookAuthor != null)
        {
            if (book.IsAuthorBook)
            {
                selectAuthorApp = bookAuthor.AppAuthorId ?? -1;
            }
            else
            {
                selectAuthor = bookAuthor.AuthorId ?? -1;
            }
        }

        // Устанавливаем имя автора
        if (selectAuthor != -1)
        {
            var author = ListsStaticClass.listAllAuthors
                .FirstOrDefault(a => a.Id == selectAuthor);
            if (author != null)
            {
                authorName.Text = author.Name;
            }
        }
        else
        {
            var user = ListsStaticClass.listAllUsers
                .FirstOrDefault(u => u.Id == selectAuthorApp);
            if (user != null)
            {
                authorName.Text = user.Login;
            }
        }

        // Находим информацию о серии
        var seriesBook = ListsStaticClass.listAllSeriesbook
            .FirstOrDefault(sb => sb.BookId == idThisBook);

        if (seriesBook != null)
        {
            select = seriesBook.SeriesId;
            orderSer.Text = seriesBook.BookOrder.ToString();

            var series = ListsStaticClass.listAllSeries
                .FirstOrDefault(s => s.Id == select);

            if (series != null)
            {
                seria.Text = series.Title;
            }
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

        ListsStaticClass.listAllSeries.Clear();
        ListsStaticClass.listAllSeries = Baza.DbContext.Series.Select(series => new Series
        {
            Id = series.Id,
            Title = series.Title,
            UserId = series.UserId,
        }).ToList();

        ListsStaticClass.listAllSeriesbook.Clear();
        ListsStaticClass.listAllSeriesbook = Baza.DbContext.Seriesbooks.Select(seriesbook => new Seriesbook
        {
            Id = seriesbook.Id,
            BookId = seriesbook.BookId,
            BookOrder = seriesbook.BookOrder,
            SeriesId = seriesbook.SeriesId,
        }).ToList();

        ListsStaticClass.listAllBookAuthors.Clear();
        ListsStaticClass.listAllBookAuthors = Baza.DbContext.Bookauthors.Select(bookA => new Bookauthor
        {
            Id = bookA.Id,
            BookId = bookA.BookId,
            AppAuthor = bookA.AppAuthor,
            AuthorId = bookA.AuthorId,
        }).ToList();

        ListsStaticClass.listAllAuthors.Clear();
        ListsStaticClass.listAllAuthors = Baza.DbContext.Authors.Select(authors => new Author
        {
            Id = authors.Id,
            Name = authors.Name,
        }).ToList();

        ListsStaticClass.listAllUsers.Clear();
        ListsStaticClass.listAllUsers = Baza.DbContext.Users.Select(user => new User
        {
            Id = user.Id,
            Login = user.Login,
        }).ToList();
    }
}