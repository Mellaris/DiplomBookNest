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
        listAllQ.ItemsSource = ListsStaticClass.listAllQuote.ToList();
    }
    private void AddNewRev(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {

    }
    private void AddNewQ(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new AddingQuotes(idThisBook).ShowDialog(this);
    }
    private void AddPersonally(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if(!string.IsNullOrEmpty(original.Text) && !string.IsNullOrEmpty(love.Text) && !string.IsNullOrEmpty(characters.Text) && !string.IsNullOrEmpty(worldInside.Text) 
            && !string.IsNullOrEmpty(ha.Text) && !string.IsNullOrEmpty(things.Text))
        {
            bool isValid = int.TryParse(original.Text, out int originalVal) &&
                      int.TryParse(love.Text, out int loveVal) &&
                      int.TryParse(characters.Text, out int charactersVal) &&
                      int.TryParse(worldInside.Text, out int worldVal) &&
                      int.TryParse(ha.Text, out int haVal) &&
                      int.TryParse(things.Text, out int thingsVal) &&
                      Enumerable.Range(1, 5).Contains(originalVal) &&
                      Enumerable.Range(1, 5).Contains(loveVal) &&
                      Enumerable.Range(1, 5).Contains(charactersVal) &&
                      Enumerable.Range(1, 5).Contains(worldVal) &&
                      Enumerable.Range(1, 5).Contains(haVal) &&
                      Enumerable.Range(1, 5).Contains(thingsVal);

            if (isValid)
            {
                float raitingAll = (Convert.ToInt32(original.Text) + Convert.ToInt32(love.Text) + Convert.ToInt32(ha.Text) 
                    + Convert.ToInt32(things.Text) + Convert.ToInt32(characters.Text) + Convert.ToInt32(worldInside.Text)) / 6;
                ListsStaticClass.listAllPersonallLibrary = ListsStaticClass.listAllPersonallLibrary.OrderBy(x => x.Id).ToList();
                var newLibrary = new Personallibrary
                {
                    Id = ListsStaticClass.listAllPersonallLibrary.LastOrDefault().Id + 1,
                    BookId = idThisBook,
                    ReaderId = ListsStaticClass.currentAccount,
                    Rating = raitingAll,
                    PlotRating = Convert.ToInt32(original.Text),
                    HumorRating = Convert.ToInt32(ha.Text),
                    CharactersRating = Convert.ToInt32(characters.Text),
                    MeaningRating = Convert.ToInt32(things.Text),
                    WorldRating = Convert.ToInt32(worldInside.Text),
                    RomanceRating = Convert.ToInt32(love.Text),
                    DateAdd = DateTime.Now,
                };
                Baza.DbContext.Personallibraries.Add(newLibrary);
                Baza.DbContext.SaveChanges();
            }
            else
            {
                string error = "Все поля должны содержать число от 1 до 5!";
                new ErrorReport(error).ShowDialog(this);
            }
        }
        else
        {
            string error = "Все поля должны быть заполнены!";
            new ErrorReport(error).ShowDialog(this);        
        }
    }
    private void CallBaza()
    {
        ListsStaticClass.listAllPersonallLibrary.Clear();
        ListsStaticClass.listAllPersonallLibrary = Baza.DbContext.Personallibraries.Select(personal => new Personallibrary
        {
            Id = personal.Id,
            BookId = personal.BookId,
            ReaderId = personal.ReaderId,
            Rating = personal.Rating,
            PlotRating = personal.PlotRating,
            HumorRating = personal.HumorRating,
            CharactersRating = personal.CharactersRating,
            MeaningRating = personal.MeaningRating,
            DateAdd = personal.DateAdd,
            Feedback = personal.Feedback,
            WorldRating = personal.WorldRating,
            RomanceRating = personal.RomanceRating,
        }).ToList();

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

        ListsStaticClass.listAllQuote.Clear();
        ListsStaticClass.listAllQuote = Baza.DbContext.Quotes.Select(quotes => new Quote
        {
            Id = quotes.Id,
            BookId= quotes.BookId,
            ReaderId = quotes.ReaderId,
            Text = quotes.Text,
        }).ToList();

        ListsStaticClass.listAllBookreview.Clear();
        ListsStaticClass.listAllBookreview = Baza.DbContext.Bookreviews.Select(Bookreview => new Bookreview
        {
            Id = Bookreview.Id,
            BookId = Bookreview.BookId,
            ReaderId= Bookreview.ReaderId,
            ReviewText = Bookreview.ReviewText,
            CreatedAt = Bookreview.CreatedAt,
            IsHaveRev = Bookreview.IsHaveRev,
        }).ToList();
    }


}