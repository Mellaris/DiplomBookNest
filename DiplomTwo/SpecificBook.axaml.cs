using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;
using System.Linq;
using DiplomTwo.Models;
using System.Collections.Generic;

namespace DiplomTwo;

public partial class SpecificBook : Window
{
    private int idThisBook;
    private int selectAuthor = -1;
    private int selectAuthorApp = -1;
    private List<Genre> genreList = new List<Genre>();
    int idGenreThis;
    public SpecificBook()
    {
        InitializeComponent();
    }
    public SpecificBook(int idForBook)
    {
        InitializeComponent();
        try
        {
            SpecificBookIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
        CallBaza();
        foreach (Book book in ListsStaticClass.listAllBooks)
        {
            if (book.Id == idForBook)
            {
                imageForBook.Source = book.CoverBitmap;
                titleForBook.Text = book.Title;
                ratingForBook.Text = book.Rating.HasValue
                ? book.Rating.Value.ToString("0.0")
                : "0.0";
                contetForBook.Text = book.Description;
                colPagesForBook.Text = book.PageCount.ToString();
                ageForBook.Text = book.AgeLimit.ToString();
                if (book.IsAuthorBook == true)
                {
                    readABook.IsVisible = true;
                }
                break;
            }
        }

        idThisBook = idForBook;
        // Считаем, сколько раз книга встречается в Personallibrary
        int readCount = Baza.DbContext.Personallibraries
            .Count(p => p.BookId == idForBook);

        // Считаем, сколько раз книга встречается в BookPlan
        int planCount = Baza.DbContext.Bookplans
            .Count(p => p.BookId == idForBook);

        // Считаем, сколько рецензий написано на книгу с пометкой IsHaveRev == true
        int reviewCount = Baza.DbContext.Bookreviews
            .Count(r => r.BookId == idForBook && r.IsHaveRev == true);

        int countQ = Baza.DbContext.Quotes
            .Count(p => p.BookId == idForBook);

        // Присваиваем значения в поля
        kolReadForBook.Text = readCount.ToString();
        kolPlanForBook.Text = planCount.ToString();
        kolRewForBook.Text = reviewCount.ToString();
        kolKVForBook.Text = countQ.ToString();
        var bookTwo = ListsStaticClass.listAllBooks.FirstOrDefault(b => b.Id == idThisBook);
        if (bookTwo is null) return;

        var bookAuthor = ListsStaticClass.listAllBookAuthors
           .FirstOrDefault(ba => ba.BookId == idThisBook);

        if (bookAuthor != null)
        {
            if (bookTwo.IsAuthorBook)
            {
                selectAuthorApp = bookAuthor.AppAuthorId ?? -1;
            }
            else
            {
                selectAuthor = bookAuthor.AuthorId ?? -1;
            }
        }
        if (selectAuthor != -1)
        {
            var author = ListsStaticClass.listAllAuthors
                .FirstOrDefault(a => a.Id == selectAuthor);
            if (author != null)
            {
                authorForBook.Text = author.Name;
            }
        }
        else
        {
            var user = ListsStaticClass.listAllUsers
                .FirstOrDefault(u => u.Id == selectAuthorApp);
            if (user != null)
            {
                authorForBook.Text = user.Login;
            }
        }
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

        var reviewsWithUsernames = (from review in Baza.DbContext.Bookreviews
                                    join user in Baza.DbContext.Users
                                    on review.ReaderId equals user.Id
                                    where review.BookId == idForBook && review.IsHaveRev == true
                                    select new
                                    {
                                        ReaderId = review.ReaderId,
                                        Login = user.Login,
                                        ReviewText = review.ReviewText,
                                        CreatedAt = review.CreatedAt.HasValue
                                            ? review.CreatedAt.Value.ToString("dd.MM.yyyy")
                                            : ""
                                    }).ToList();

        // Привязка к ListBox
        listForBookAndRev.ItemsSource = reviewsWithUsernames.ToList();
        var quotesForBook = Baza.DbContext.Quotes
            .Where(q => q.BookId == idForBook)
            .Select(q => new
            {
                Text = q.Text
            }).ToList();

        listForBookAndQ.ItemsSource = quotesForBook.ToList();
        SortForBookSame();
    }
    private void SortForBookSame()
    {
        // Получаем список жанров текущей книги
        List<int> genreIdsForThisBook = ListsStaticClass.listAllBookGenres
            .Where(bg => bg.BookId == idThisBook)
            .Select(bg => bg.GenreId)
            .Distinct()
            .ToList();

        // Находим книги с количеством совпадений по жанрам
        var similarBookMatches = ListsStaticClass.listAllBookGenres
            .Where(bg => bg.BookId != idThisBook && genreIdsForThisBook.Contains(bg.GenreId))
            .GroupBy(bg => bg.BookId)
            .Select(group => new
            {
                BookId = group.Key,
                MatchCount = group.Count()
            })
            .OrderByDescending(x => x.MatchCount)
            .Take(10)
            .ToList();

        // Получаем сами книги
        List<Book> similarBooks = ListsStaticClass.listAllBooks
            .Where(book => similarBookMatches.Any(m => m.BookId == book.Id))
            .ToList();

        // Привязка к ListBox
        sameBook.ItemsSource = similarBooks.ToList();

    }
    private void Add(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        
        if (ListsStaticClass.currentAccount != -1)
        {
            new WritingReview(idThisBook).Show();
            Close();
        }
        else
        {
            string error = "Вы должны войти в аккаунт, чтобы воспользоваться этой функцией!";
            new ErrorReport(error).ShowDialog(this);
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

        ListsStaticClass.listAllAuthors.Clear();
        ListsStaticClass.listAllAuthors = Baza.DbContext.Authors.Select(author => new Author
        {
            Id = author.Id,
            Biography = author.Biography,
            Name = author.Name,
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

    private void Back(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }

    private void OpenBooks(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Books().Show();
        Close();
    }

    private void OpenAuthors(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if(ListsStaticClass.currentAccount != -1)
        {
            new Friends().Show();
            Close();
        }
        else
        {
            string error = "Вы должны войти в аккаунт!";
            new ErrorReport(error).ShowDialog(this);
        }
    }

    private void OpenReadABook(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (ListsStaticClass.currentAccount != -1)
        {
            new ReadBook(idThisBook).Show();
            Close();
        }
        else
        {
            string error = "Вы должны войти в аккаунт, чтобы воспользоваться этой функцией!";
            new ErrorReport(error).ShowDialog(this);

        }

    }

    private void My(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (ListsStaticClass.currentAccount != -1)
        {
            new personalAccount().Show();
            Close();
        }
        else
        {
           new LogIn().Show();
           Close();
        }
        
    }

    private void AddPlanNew(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (ListsStaticClass.currentAccount != -1)
        {
            new AddingReadOrPlan(idThisBook).ShowDialog(this);
        }
        else
        {
            string error = "Вы должны войти в аккаунт, чтобы воспользоваться этой функцией!";
            new ErrorReport(error).ShowDialog(this);
        }
    }

    private void AddRev(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (ListsStaticClass.currentAccount != -1)
        {
            new WritingReview(idThisBook).Show();
            Close();
        }
        else
        {
            string error = "Вы должны войти в аккаунт, чтобы воспользоваться этой функцией!";
            new ErrorReport(error).ShowDialog(this);
        }
    }

    private void AddQ(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (ListsStaticClass.currentAccount != -1)
        {
            new WritingReview(idThisBook).Show();
            Close();
        }
        else
        {
            string error = "Вы должны войти в аккаунт, чтобы воспользоваться этой функцией!";
            new ErrorReport(error).ShowDialog(this);
        }
    }

    private void ListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        var selectedBook = sameBook.SelectedItem as Book;

        if (selectedBook != null)
        {
            int bookId = selectedBook.Id; // Получаем id книги
            new SpecificBook(bookId).Show();
            Close();
        }
    }
}