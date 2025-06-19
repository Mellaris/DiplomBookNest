using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Linq;
using DiplomTwo.Models;
using System;
using System.Collections.Generic;
namespace DiplomTwo;

public partial class Moderation : Window
{
    private int oneBoxSelect;
    private int oneBoxSelectBooks;
    private int twoBoxSelectBooks;
    private int threeBoxSelectBooks;
    private int threeBoxSelect;
    public Moderation()
    {
        InitializeComponent();
        CallBaza();
        allBooksOne.ItemsSource = ListsStaticClass.listAllBooks.OrderBy(x => x.Id).ToList();
        allBooksTwo.ItemsSource = ListsStaticClass.listAllBooks.OrderBy(x => x.Id).ToList();
        
        AllContent();
    }
    private void AllContent()
    {
        var users = Baza.DbContext.Users.ToList();
        var books = Baza.DbContext.Books.ToList();

        var reviewsDisplay = ListsStaticClass.listAllBookreview
        .Where(r => Convert.ToBoolean(r.IsHaveRev) && !string.IsNullOrWhiteSpace(r.ReviewText)) // только те, у кого есть текст и IsHaveRev == true
        .Select(r => new ReviewDisplayModel
        {
            Id = r.Id,
            BookId = Convert.ToInt32(r.BookId),
            Login = users.FirstOrDefault(u => u.Id == r.ReaderId)?.Login ?? "Неизвестно",
            BookTitle = books.FirstOrDefault(b => b.Id == r.BookId)?.Title ?? "Неизвестно",
            ReviewText = r.ReviewText,
            CreatedAt = r.CreatedAt
        })
        .ToList();
        allReviewsListBox.ItemsSource = reviewsDisplay.ToList();

        var quotesDisplay = ListsStaticClass.listAllQuote
        .Select(q => new QuoteDisplayModel
        {
            Id = q.Id,
            BookId = Convert.ToInt32(q.BookId),
            Login = users.FirstOrDefault(u => u.Id == q.ReaderId)?.Login ?? "Неизвестно",
            BookTitle = books.FirstOrDefault(b => b.Id == q.BookId)?.Title ?? "Неизвестно",
            QuoteText = q.Text
        })
        .ToList();
        allQuotesListBox.ItemsSource = quotesDisplay.ToList();

        var commentsDisplay = ListsStaticClass.listAllComment
        .Select(c => new CommentDisplayModel
        {
            Id = c.Id,
            BookId = Convert.ToInt32(c.BookId),
            Login = users.FirstOrDefault(u => u.Id == c.ReaderId)?.Login ?? "Неизвестно",
            BookTitle = books.FirstOrDefault(b => b.Id == c.BookId)?.Title ?? "Неизвестно",
            Content = c.Content,
            CreatedAt = c.CreatedAt
        })
        .ToList();
        allCommentsListBox.ItemsSource = commentsDisplay.ToList();
    }

    private void CallBaza()
    {
        ListsStaticClass.listAllBooks.Clear();
        ListsStaticClass.listAllBooks = Baza.DbContext.Books.Select(book => new Book
        {
            Id = book.Id,
            Title = book.Title,
        }).ToList();

        ListsStaticClass.listAllBookreview.Clear();
        ListsStaticClass.listAllBookreview = Baza.DbContext.Bookreviews.Select(book => new Bookreview
        {
            Id = book.Id,
            BookId = book.BookId,
            ReaderId = book.ReaderId,
            CreatedAt = book.CreatedAt,
            IsHaveRev = book.IsHaveRev,
            ReviewText = book.ReviewText,
        }).ToList();

        ListsStaticClass.listAllComment.Clear();
        ListsStaticClass.listAllComment = Baza.DbContext.Comments.Select(coment => new Comment
        {
            Id = coment.Id,
            CreatedAt = coment.CreatedAt,
            BookId = coment.BookId,
            ReaderId = coment.ReaderId,
            Content = coment.Content,
        }).ToList();

        ListsStaticClass.listAllQuote.Clear();
        ListsStaticClass.listAllQuote = Baza.DbContext.Quotes.Select(q => new Quote
        {
            Id = q.Id,
            ReaderId = q.ReaderId,
            Text = q.Text,
            BookId = q.BookId,
        }).ToList();
    }

    private void Back(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new AdminPanel().Show();
        Close();
    }
    private void DispleyOne()
    {
        var filteredReviews = ListsStaticClass.listAllBookreview
            .Where(r => Convert.ToBoolean(r.IsHaveRev) && !string.IsNullOrWhiteSpace(r.ReviewText))
            .ToList();

        // фильтрация по книге, если выбрана
        if (oneBoxSelectBooks != 0)
        {
            filteredReviews = filteredReviews
                .Where(r => r.BookId == oneBoxSelectBooks)
                .ToList();
        }

        // сортировка по дате
        if (oneBoxSelect == 1)
        {
            filteredReviews = filteredReviews
                .OrderBy(r => r.CreatedAt)
                .ToList();
        }
        else if (oneBoxSelect == 2)
        {
            filteredReviews = filteredReviews
                .OrderByDescending(r => r.CreatedAt)
                .ToList();
        }

        var users = Baza.DbContext.Users.ToList();
        var books = Baza.DbContext.Books.ToList();

        var displayList = filteredReviews.Select(r => new ReviewDisplayModel
        {
            Id = r.Id,
            BookId = Convert.ToInt32(r.BookId),
            Login = users.FirstOrDefault(u => u.Id == r.ReaderId)?.Login ?? "Неизвестно",
            BookTitle = books.FirstOrDefault(b => b.Id == r.BookId)?.Title ?? "Неизвестно",
            ReviewText = r.ReviewText,
            CreatedAt = r.CreatedAt
        }).ToList();

        allReviewsListBox.ItemsSource = displayList.ToList();
    }

    private void BoxForDateAndReviews(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        oneBoxSelect = (sender as ComboBox)?.SelectedIndex + 1 ?? 0;
        DispleyOne();
    }

    private void BoxForBooksAndReviews(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        if (allBooksOne.SelectedItem is Book selectedBook)
        {
            oneBoxSelectBooks = selectedBook.Id;
        }
        else
        {
            oneBoxSelectBooks = 0; // если ничего не выбрано — показать все
        }

        DispleyOne();
    }

    private void DeleteReviews(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        int select = (int)(sender as Button).Tag;
        var selectContent = Baza.DbContext.Bookreviews.FirstOrDefault(x => x.Id == select);
        if (selectContent != null)
        {
            Baza.DbContext.Bookreviews.Remove(selectContent);
            Baza.DbContext.SaveChanges();

            ListsStaticClass.listAllBookreview.RemoveAll(x => x.Id == select);
            DispleyOne();
        }
    }

    private void BoxForBooksAndQut(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        List<QuoteDisplayModel> filteredQuotes;

        if (allBooksTwo.SelectedItem is Book selectedBook)
        {
            twoBoxSelectBooks = selectedBook.Id;

            filteredQuotes = ListsStaticClass.listAllQuote
                .Where(q => q.BookId == twoBoxSelectBooks)
                .Select(q => new QuoteDisplayModel
                {
                    Id = q.Id,
                    Login = ListsStaticClass.listAllUsers.FirstOrDefault(u => u.Id == ListsStaticClass.listAllReaders.FirstOrDefault(r => r.IdReader == q.ReaderId)?.UserId)?.Login,
                    BookTitle = ListsStaticClass.listAllBooks.FirstOrDefault(b => b.Id == q.BookId)?.Title,
                    QuoteText = q.Text,
                    BookId = Convert.ToInt32(q.BookId),
                })
                .ToList();
        }
        else
        {
            twoBoxSelectBooks = 0;

            filteredQuotes = ListsStaticClass.listAllQuote
                .Select(q => new QuoteDisplayModel
                {
                    Id = q.Id,
                    Login = ListsStaticClass.listAllUsers.FirstOrDefault(u => u.Id == ListsStaticClass.listAllReaders.FirstOrDefault(r => r.IdReader == q.ReaderId)?.UserId)?.Login,
                    BookTitle = ListsStaticClass.listAllBooks.FirstOrDefault(b => b.Id == q.BookId)?.Title,
                    QuoteText = q.Text,
                    BookId = Convert.ToInt32(q.BookId)
                })
                .ToList();
        }

        // Показываем результат в нужном ListBox
        allQuotesListBox.ItemsSource = filteredQuotes.ToList();
    }
    private void DisplayTwo()
    {
        var filteredReviews = ListsStaticClass.listAllComment
            .Where(r => !string.IsNullOrWhiteSpace(r.Content))
            .ToList();


        var users = Baza.DbContext.Users.ToList();
        var books = Baza.DbContext.Books.ToList();

        var displayList = filteredReviews.Select(r => new CommentDisplayModel
        {
            Id = r.Id,
            BookId = Convert.ToInt32(r.BookId),
            Login = users.FirstOrDefault(u => u.Id == r.ReaderId)?.Login ?? "Неизвестно",
            BookTitle = books.FirstOrDefault(b => b.Id == r.BookId)?.Title ?? "Неизвестно",
            Content = r.Content,
            CreatedAt = r.CreatedAt
        }).ToList();

        allCommentsListBox.ItemsSource = displayList.ToList();
    }
   
    private void DeleteQ(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        int select = (int)(sender as Button).Tag;
        var selectContent = Baza.DbContext.Quotes.FirstOrDefault(x => x.Id == select);
        if (selectContent != null)
        {
            Baza.DbContext.Quotes.Remove(selectContent);
            Baza.DbContext.SaveChanges();

            ListsStaticClass.listAllQuote.RemoveAll(x => x.Id == select);
            BoxForBooksAndQut(null, null); // Обновляем цитаты
        }
    }
    private void DeleteComment(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        int select = (int)(sender as Button).Tag;
        var selectContent = Baza.DbContext.Comments.FirstOrDefault(x => x.Id == select);
        if (selectContent != null)
        {
            Baza.DbContext.Comments.Remove(selectContent);
            Baza.DbContext.SaveChanges();

            ListsStaticClass.listAllComment.RemoveAll(x => x.Id == select);
            DisplayTwo(); // Обновляем комментарии
        }
    }
}
public class ReviewDisplayModel
{
    public int Id { get; set; }             // ID рецензии
    public int BookId { get; set; }         // ID книги
    public string Login { get; set; }       // Логин пользователя
    public string BookTitle { get; set; }   // Название книги
    public string ReviewText { get; set; }  // Текст рецензии
    public DateTime? CreatedAt { get; set; } // Дата создания
}
public class QuoteDisplayModel
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string Login { get; set; }
    public string BookTitle { get; set; }
    public string QuoteText { get; set; }
}
public class CommentDisplayModel
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string Login { get; set; }
    public string BookTitle { get; set; }
    public string Content { get; set; }
    public DateTime? CreatedAt { get; set; }
}
