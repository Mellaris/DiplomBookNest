using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;
using System.Linq;
using DiplomTwo.Models;
using System.Collections.Generic;

namespace DiplomTwo;

public partial class WritingReview : Window
{
    private int idThisBook;
    private int select;
    private int selectAuthor = -1;
    private int selectAuthorApp = -1;
    private List<Genre> genreList = new List<Genre>();
    int idGenreThis;
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
        RefreshQuotesList();
        foreach (BookGenre bookGenre in ListsStaticClass.listAllBookGenres)
        {
            if (bookGenre.BookId == idThisBook)
            {
                idGenreThis = bookGenre.GenreId;
                foreach (Genre genre in ListsStaticClass.listAllGenres)
                {
                    if (genre.Id == idGenreThis)
                    {
                        genreList.Add(genre);
                        break;
                    }
                }

            }
        }
        listForGenre.ItemsSource = genreList.ToList();
        foreach (Bookreview bookreview in ListsStaticClass.listAllBookreview)
        {
            if (bookreview.BookId == idThisBook && bookreview.ReaderId == ListsStaticClass.currentAccount)
            {
                textForRevu.Text = bookreview.ReviewText;
                if (bookreview.IsHaveRev == false)
                {
                    buttonForRev.IsVisible = true;
                }
                else
                {
                    CloseButton.IsVisible = true;
                }
            }
        }
        foreach (Personallibrary personallibrary in ListsStaticClass.listAllPersonallLibrary)
        {
            if (personallibrary.BookId == idThisBook && personallibrary.ReaderId == ListsStaticClass.currentAccount)
            {
                original.Text = personallibrary.PlotRating.ToString();
                love.Text = personallibrary.RomanceRating.ToString();
                ha.Text = personallibrary.HumorRating.ToString();
                characters.Text = personallibrary.CharactersRating.ToString();
                worldInside.Text = personallibrary.WorldRating.ToString();
                things.Text = personallibrary.MeaningRating.ToString();
                rating.Text = personallibrary.Rating.ToString();
                break;
            }
        }
    }
    public void RefreshQuotesList()
    {
        ListsStaticClass.listAllQuote.Clear();
        listAllQ.ItemsSource = ListsStaticClass.listAllQuote.ToList();
        CallBaza();
        ListsStaticClass.listAllQuote = Baza.DbContext.Quotes
            .Where(q => q.ReaderId == ListsStaticClass.currentAccount && q.BookId == idThisBook)
            .ToList();

        listAllQ.ItemsSource = ListsStaticClass.listAllQuote.ToList();
    }

    private void AddNewRev(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(textForRevu.Text))
        {
            int currentUserId = ListsStaticClass.currentAccount;

            var existingReview = Baza.DbContext.Bookreviews
                .FirstOrDefault(r => r.BookId == idThisBook && r.ReaderId == currentUserId);

            if (existingReview != null)
            {
                // Обновление существующей рецензии
                existingReview.ReviewText = textForRevu.Text;
            }
            else
            {
                // Создание новой рецензии
                var newReview = new Bookreview
                {
                    BookId = idThisBook,
                    ReaderId = currentUserId,
                    ReviewText = textForRevu.Text,
                    CreatedAt = DateTime.Now, // если у тебя есть поле "дата"
                    IsHaveRev = false,
                };
                Baza.DbContext.Bookreviews.Add(newReview);
            }
            buttonForRev.IsVisible = true;
            Baza.DbContext.SaveChanges();
        }
    }
    private void AddOpen(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        int currentUserId = ListsStaticClass.currentAccount;

        var existingReview = Baza.DbContext.Bookreviews
            .FirstOrDefault(r => r.BookId == idThisBook && r.ReaderId == currentUserId);
        if (existingReview != null)
        {
            // Обновление существующей рецензии
            existingReview.IsHaveRev = true;
            Baza.DbContext.SaveChanges();
            CloseButton.IsVisible = true;
            buttonForRev.IsVisible = false;
        }
    }
    private void CloseAdd(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        int currentUserId = ListsStaticClass.currentAccount;

        var existingReview = Baza.DbContext.Bookreviews
            .FirstOrDefault(r => r.BookId == idThisBook && r.ReaderId == currentUserId);
        if (existingReview != null)
        {
            // Обновление существующей рецензии
            existingReview.IsHaveRev = false;
            Baza.DbContext.SaveChanges();
            CloseButton.IsVisible = false;
            buttonForRev.IsVisible = true;
        }
    }

    private void AddNewQ(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var addQuoteWindow = new AddingQuotes(idThisBook, this);
        addQuoteWindow.ShowDialog(this);
    }
    private void AddPersonally(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(original.Text) && !string.IsNullOrEmpty(love.Text) && !string.IsNullOrEmpty(characters.Text) &&
            !string.IsNullOrEmpty(worldInside.Text) && !string.IsNullOrEmpty(ha.Text) && !string.IsNullOrEmpty(things.Text))
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
                + Convert.ToInt32(things.Text) + Convert.ToInt32(characters.Text) + Convert.ToInt32(worldInside.Text)) / 6f;

                raitingAll = (float)Math.Round(raitingAll, 1);

                var existingEntry = Baza.DbContext.Personallibraries
                .FirstOrDefault(p => p.BookId == idThisBook && p.ReaderId == ListsStaticClass.currentAccount);

                if (existingEntry != null)
                {
                    // Обновляем существующую запись
                    existingEntry.Rating = raitingAll;
                    existingEntry.PlotRating = Convert.ToInt32(original.Text);
                    existingEntry.HumorRating = Convert.ToInt32(ha.Text);
                    existingEntry.CharactersRating = Convert.ToInt32(characters.Text);
                    existingEntry.MeaningRating = Convert.ToInt32(things.Text);
                    existingEntry.WorldRating = Convert.ToInt32(worldInside.Text);
                    existingEntry.RomanceRating = Convert.ToInt32(love.Text);
                    existingEntry.DateAdd = DateTime.Now;
                    rating.Text = existingEntry.Rating.ToString();
                }
                else
                {
                    ListsStaticClass.listAllPersonallLibrary = ListsStaticClass.listAllPersonallLibrary.OrderBy(x => x.Id).ToList();
                    // Добавляем новую запись
                    var newLibrary = new Personallibrary
                    {
                        Id = ListsStaticClass.listAllPersonallLibrary.LastOrDefault()?.Id + 1 ?? 1,
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
                }

                Baza.DbContext.SaveChanges();
                var bookPlanEntry = Baza.DbContext.Bookplans
                .FirstOrDefault(bp => bp.BookId == idThisBook && bp.ReaderId == ListsStaticClass.currentAccount);

                if (bookPlanEntry != null)
                {
                    Baza.DbContext.Bookplans.Remove(bookPlanEntry);
                    Baza.DbContext.SaveChanges();

                    // Обновим локальный список после удаления
                    ListsStaticClass.listAllBookPlan = Baza.DbContext.Bookplans.ToList();
                }

                // Обновляем рейтинг книги
                var allRatingsForBook = Baza.DbContext.Personallibraries
                    .Where(p => p.BookId == idThisBook)
                    .Select(p => p.Rating)
                    .ToList();

                if (allRatingsForBook.Count > 0)
                {
                    var roundedRatings = allRatingsForBook
                        .Where(r => r.HasValue)
                        .Select(r => (float)Math.Round(r.Value, 1))
                        .ToList();

                    float averageRating = (float)Math.Round(roundedRatings.Average(), 1);

                    var thisBook = Baza.DbContext.Books.FirstOrDefault(b => b.Id == idThisBook);
                    if (thisBook != null)
                    {
                        thisBook.Rating = averageRating;
                        Baza.DbContext.SaveChanges();
                    }

                    var localBook = ListsStaticClass.listAllBooks.FirstOrDefault(b => b.Id == idThisBook);
                    if (localBook != null)
                    {
                        localBook.Rating = averageRating;
                    }
                }

                // Обновим локальный список
                ListsStaticClass.listAllPersonallLibrary = Baza.DbContext.Personallibraries.ToList();
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
        CallBaza();
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
            BookId = quotes.BookId,
            ReaderId = quotes.ReaderId,
            Text = quotes.Text,
        }).ToList();

        ListsStaticClass.listAllBookreview.Clear();
        ListsStaticClass.listAllBookreview = Baza.DbContext.Bookreviews.Select(Bookreview => new Bookreview
        {
            Id = Bookreview.Id,
            BookId = Bookreview.BookId,
            ReaderId = Bookreview.ReaderId,
            ReviewText = Bookreview.ReviewText,
            CreatedAt = Bookreview.CreatedAt,
            IsHaveRev = Bookreview.IsHaveRev,
        }).ToList();

        ListsStaticClass.listAllBookGenres.Clear();
        ListsStaticClass.listAllBookGenres = Baza.DbContext.BookGenres.Select(bookGenre => new BookGenre
        {
            Id = bookGenre.Id,
            BookId = bookGenre.BookId,
            GenreId = bookGenre.GenreId,
        }).ToList();

        ListsStaticClass.listAllGenres.Clear();
        ListsStaticClass.listAllGenres = Baza.DbContext.Genres.Select(genre => new Genre
        {
            Id = genre.Id,
            Name = genre.Name,
        }).ToList();

    }

    private void Home(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }

    private void AllBookOpen(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
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