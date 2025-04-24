using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Linq;
using DiplomTwo.Models;
using System.Collections.Generic;
using Avalonia.Media.Imaging;
using System;

namespace DiplomTwo;

public partial class CreatingBook : Window
{
    private List<BookChapter> bookChapterList = new List<BookChapter>();
    public CreatingBook()
    {
        InitializeComponent();
    }
    public CreatingBook(int idThisBook)
    {
        InitializeComponent();
        try
        {
            CreatingBookIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
        CallBaza();
        int idForBook = idThisBook;
        foreach (BookChapter chapter in ListsStaticClass.listAllBookChapter)
        {
            if(idForBook == chapter.BookId)
            {
                bookChapterList.Add(chapter);
            }
        }
        listForChapters.ItemsSource = bookChapterList.ToList();
    }
    private void AddNewChapter(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {

    }
    private void CallBaza()
    {
        ListsStaticClass.listAllBookChapter.Clear();
        ListsStaticClass.listAllBookChapter = Baza.DbContext.BookChapters.Select(book => new BookChapter
        {
            Id = book.Id,
            Title = book.Title,
            ChapterNumber = book.ChapterNumber,
            Content = book.Content,
            CreatedAt = book.CreatedAt,
            UpdatedAt = book.UpdatedAt,
            WordCount = book.WordCount,
            BookId = book.BookId,
        }).ToList();
    }
    private void MyAccount(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new personalAccount().Show();
        Close();
    }

    private void Log(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new LogIn().Show();
        Close();
    }

    private void AllBooks(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Books().Show();
        Close();
    }

    
}