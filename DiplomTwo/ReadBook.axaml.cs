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
        foreach(BookChapter bookChapter in ListsStaticClass.listAllBookChapter)
        {
            if(bookChapter.BookId == idBookThis)
            {
                bookChaptersThisBook.Add(bookChapter);
            }
        }
        AllChapter.ItemsSource = bookChaptersThisBook.ToList();
    }
    private void ListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        if(AllChapter.SelectedItem != null)
        {
            var selectedChapter = AllChapter.SelectedItem as BookChapter;
            if (selectedChapter != null)
            {
                int chapterId = selectedChapter.Id; // Получаем id главы
                OpenNewChapter(chapterId);
            }
        }
    }
    private void OpenNewChapter(int idThisChaper)
    {
        foreach(BookChapter bookChapter in bookChaptersThisBook)
        {
            if(bookChapter.Id == idThisChaper)
            {
                titleChapter.Text = bookChapter.Title;
                contentChapter.Text = bookChapter.Content;
                break;
            }
        }
        foreach(Comment comment in ListsStaticClass.listAllComment)
        {
            if(comment.ChapterId == idThisChaper)
            {
                commentsThisBook.Add(comment);
            }
        }
        AllComment.ItemsSource = commentsThisBook.ToList();
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

  
}