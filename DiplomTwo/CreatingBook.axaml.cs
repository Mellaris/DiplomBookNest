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
    private int idForBook;
    private int wordsPerPage = 325;
    private int selectIdChapterNumber;
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
        idForBook = idThisBook;
        foreach (BookChapter chapter in ListsStaticClass.listAllBookChapter)
        {
            if(idForBook == chapter.BookId)
            {
                bookChapterList.Add(chapter);
            }
        }
        listForChapters.ItemsSource = bookChapterList.ToList();
        foreach(Book book in ListsStaticClass.listAllBooks)
        {
            if(idForBook == book.Id)
            {
                nameBook.Text = book.Title;
            }
        }
    }
    private void AddNewChapter(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // Получаем все главы из базы
        var allChapters = Baza.DbContext.BookChapters.ToList();

        // Новый уникальный Id для главы
        int newId = allChapters.Count == 0 ? 1 : allChapters.Max(ch => ch.Id) + 1;

        // Определяем номер главы для текущей книги
        var chaptersForThisBook = allChapters.Where(ch => ch.BookId == idForBook).ToList();
        int newChapterNumber = chaptersForThisBook.Count == 0 ? 1 : chaptersForThisBook.Max(ch => ch.ChapterNumber) + 1;

        // Создаем главу
        var newChapter = new BookChapter
        {
            Id = newId,
            Title = "Название главы",
            ChapterNumber = newChapterNumber,
            Content = "",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            WordCount = 0,
            BookId = idForBook
        };

        // Сохраняем в базу данных
        Baza.DbContext.BookChapters.Add(newChapter);
        Baza.DbContext.SaveChanges();

        // Обновляем локальный список и перерисовываем лист
        ListsStaticClass.listAllBookChapter.Add(newChapter);
        bookChapterList.Add(newChapter);
        listForChapters.ItemsSource = null;
        listForChapters.ItemsSource = bookChapterList.ToList();

       
    }
    private void DeleteThisChapter(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        int selectDeleteIdChapter = (int)(sender as Button).Tag;
        // Находим удаляемую главу
        var chapterToDelete = Baza.DbContext.BookChapters.FirstOrDefault(ch => ch.Id == selectDeleteIdChapter);
        if (chapterToDelete == null) return;

        int deletedChapterNumber = chapterToDelete.ChapterNumber;
        int bookId = idForBook;

        // Удаляем из базы
        Baza.DbContext.BookChapters.Remove(chapterToDelete);

        // Обновляем номера глав с большим номером
        var chaptersToUpdate = Baza.DbContext.BookChapters
            .Where(ch => ch.BookId == bookId && ch.ChapterNumber > deletedChapterNumber)
            .ToList();

        foreach (var chapter in chaptersToUpdate)
        {
            chapter.ChapterNumber--;
        }

        Baza.DbContext.SaveChanges();

        // Удаляем из локального списка и обновляем интерфейс
        bookChapterList.RemoveAll(ch => ch.Id == selectDeleteIdChapter);
        foreach (var ch in bookChapterList)
        {
            if (ch.BookId == bookId && ch.ChapterNumber > deletedChapterNumber)
            {
                ch.ChapterNumber--;
            }
        }

        listForChapters.ItemsSource = null;
        listForChapters.ItemsSource = bookChapterList.ToList();
    }
    private void OpenOurChapter(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        panelForText.IsVisible = true;
        textForTitle.IsEnabled = true;
        textForContent.IsEnabled = true;

        selectIdChapterNumber = (int)(sender as Button).Tag;

        foreach (BookChapter bookChapter in bookChapterList)
        {
            if (bookChapter.Id == selectIdChapterNumber)
            {
                dateUpdateLast.Text = bookChapter.UpdatedAt.ToString();
                countWordLast.Text = bookChapter.WordCount.ToString();
                textForTitle.Text = bookChapter.Title.ToString();
                textForContent.Text = bookChapter.Content.ToString();
            }
        }
    }
    private void UpdateOurChapter(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        string newTitle = textForTitle.Text;
        string newContent = textForContent.Text;

        int wordCount = 0;
        if (!string.IsNullOrEmpty(newContent))
        {
            wordCount = newContent.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        // Находим нужную главу в списке и обновляем
        var chapter = bookChapterList.FirstOrDefault(ch => ch.Id == selectIdChapterNumber);
        if (chapter != null)
        {
            chapter.Title = newTitle;
            chapter.Content = newContent;
            chapter.UpdatedAt = DateTime.Now;
            chapter.WordCount = wordCount;

            // Обновляем в БД
            var dbChapter = Baza.DbContext.BookChapters.FirstOrDefault(ch => ch.Id == selectIdChapterNumber);
            if (dbChapter != null)
            {
                dbChapter.Title = newTitle;
                dbChapter.Content = newContent;
                dbChapter.UpdatedAt = chapter.UpdatedAt;
                dbChapter.WordCount = wordCount;
                Baza.DbContext.SaveChanges();
            }

            // Обновляем UI
            dateUpdateLast.Text = chapter.UpdatedAt.ToString();
            countWordLast.Text = wordCount.ToString();
            countPageLast.Text = Math.Ceiling((double)wordCount / wordsPerPage).ToString();
        }
        bookChapterList.Clear();
        foreach (BookChapter chapterOne in ListsStaticClass.listAllBookChapter)
        {
            if (idForBook == chapterOne.BookId)
            {
                bookChapterList.Add(chapterOne);
            }
        }
        listForChapters.ItemsSource = bookChapterList.ToList();
    }
    private void TextBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        string text = (sender as TextBox).Text;
        if(!string.IsNullOrEmpty(text))
        {
            // Разделение текста по пробелам, исключая пустые элементы
            int wordCount = text.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;

            // Вычисление количества страниц
            int pageCount = (int)Math.Ceiling((double)wordCount / wordsPerPage);

            // Можно обновить UI (например, TextBlock или Label)
            countWordLast.Text = wordCount.ToString();
            countPageLast.Text = pageCount.ToString();
        }
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
        }).ToList();
    }
    private void MyAccount(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new personalAccount().Show();
        Close();
    }

    private void Log(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new personalAccount().Show();
        Close();
    }

    private void AllBooks(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Books().Show();
        Close();
    }

    private void NewWorld(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new SurroundingWorld(idForBook).Show();
        Close();
    }

    private void OpenChar(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new CharacterСreation(idForBook).Show();
        Close();
    }

    private void OpenSetting(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new SettingBook(idForBook).Show();
        Close();
    }
}