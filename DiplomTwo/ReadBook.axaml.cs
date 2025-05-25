using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;
using System.Linq;
using DiplomTwo.Models;
using System.Collections.Generic;

namespace DiplomTwo;

public partial class ReadBook : Window
{
    private List<BookChapter> bookChaptersThisBook = new List<BookChapter>();
    private List<Comment> commentsThisBook = new List<Comment>();
    private int chapterIdThisBook = -1;
    private int idBookThisHere;
    public ReadBook()
    {
        InitializeComponent();
    }
    public ReadBook(int idBookThis)
    {
        InitializeComponent();
        try
        {
            ReadBookIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
        CallBaza();
        ListsStaticClass.listAllBookChapter.OrderBy(x => x.Id).ToList();
        foreach(BookChapter bookChapter in ListsStaticClass.listAllBookChapter)
        {
            if(bookChapter.BookId == idBookThis)
            {
                bookChaptersThisBook.Add(bookChapter);
            }
        }
        bookChaptersThisBook = bookChaptersThisBook.OrderBy(x => x.Id).ToList();
        AllChapter.ItemsSource = bookChaptersThisBook.ToList();
        idBookThisHere = idBookThis;
    }
    private void AddNewComment(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if(chapterIdThisBook != -1)
        {
            if (!string.IsNullOrEmpty(textForComment.Text))
            {
                ListsStaticClass.listAllComment = ListsStaticClass.listAllComment.OrderBy(x => x.Id).ToList();
                var newComment = new Comment
                {
                    Id = ListsStaticClass.listAllComment.LastOrDefault().Id + 1,
                    ChapterId = chapterIdThisBook,
                    ReaderId = ListsStaticClass.currentAccount,
                    Content = textForComment.Text,
                    CreatedAt = DateTime.Now,
                    BookId = idBookThisHere,
                };
                Baza.DbContext.Comments.Add(newComment);
                Baza.DbContext.SaveChanges();

                CallBaza();
                OpenAllComment(chapterIdThisBook);
                textForComment.Text = null;
            }
            else
            {
                string error = "Вы должны что-нибудь написать, чтобы отправить комментарий!";
                new ErrorReport(error).ShowDialog(this);
            }
        }
        else
        {
            string error = "Вы должны выбрать главу, чтобы воспользоваться этой функцией!";
            new ErrorReport(error).ShowDialog(this);
        }
       
    }
    private void ListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        if(AllChapter.SelectedItem != null)
        {
            var selectedChapter = AllChapter.SelectedItem as BookChapter;
            if (selectedChapter != null)
            {
                chapterIdThisBook = selectedChapter.Id; // Получаем id главы
                OpenNewChapter(chapterIdThisBook);
            }
        }
    }
    private void OpenAllComment(int chapterId)
    {
        var commentsWithUsers = ListsStaticClass.listAllComment
            .Where(c => c.ChapterId == chapterId)
            .Select(c => new CommentDisplay
            {
                Text = c.Content,
                UserLogin = ListsStaticClass.listAllUsers.FirstOrDefault(u => u.Id == c.ReaderId)?.Login ?? "Неизвестно",
                DateAdd = c.CreatedAt,
            })
            .ToList();

        AllComment.ItemsSource = commentsWithUsers.ToList();
    }

    private void OpenNewChapter(int idThisChaper)
    {
        int id = idThisChaper;
        foreach(BookChapter bookChapter in bookChaptersThisBook)
        {
            if(bookChapter.Id == idThisChaper)
            {
                titleChapter.Text = bookChapter.Title;
                contentChapter.Text = bookChapter.Content;
                break;
            }
        }
        OpenAllComment(id);
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

        ListsStaticClass.listAllComment.Clear();
        ListsStaticClass.listAllComment = Baza.DbContext.Comments.Select(comment => new Comment
        {
            Id = comment.Id,
            Content = comment.Content,
            ChapterId = comment.ChapterId,
            ReaderId = comment.ReaderId,
            CreatedAt = comment.CreatedAt,
        }).ToList();

        ListsStaticClass.listAllBookChapter.Clear();
        ListsStaticClass.listAllBookChapter = Baza.DbContext.BookChapters.Select(bookChapter => new BookChapter
        {
            Id = bookChapter.Id,
            ChapterNumber = bookChapter.ChapterNumber,
            Content = bookChapter.Content,
            Title = bookChapter.Title,
            CreatedAt = bookChapter.CreatedAt,
            UpdatedAt = bookChapter.UpdatedAt,
            BookId = bookChapter.BookId,
        }).ToList();

        ListsStaticClass.listAllUsers.Clear();
        ListsStaticClass.listAllUsers = Baza.DbContext.Users.Select(user => new User
        {
            Id = user.Id,
            Login = user.Login,
        }).ToList();
    }

    private void GoToHome(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }

    private void MyAccount(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new personalAccount().Show();
        Close();
    }

    private void OpenLookComment(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        AllComment.IsVisible = true;
        commentPanel.IsVisible = true;
    }
    private void CloseLookComment(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        AllComment.IsVisible = false;
        commentPanel.IsVisible = false;
    }

    private void OpenB(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Books().Show();
        Close();
    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Friends().Show();
        Close();
    }

    private void Button_Click_2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new SpecificBook(idBookThisHere).Show();
        Close();
    }
}
public class CommentDisplay
{
    public string Text { get; set; }
    public string UserLogin { get; set; }
    public DateTime? DateAdd { get; set; }
}
