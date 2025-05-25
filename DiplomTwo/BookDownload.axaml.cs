using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using DiplomTwo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DiplomTwo;

public partial class BookDownload : Window
{
    private List<Series> seriesList = new List<Series>();
    private string selectedImageFileName = null;
    int selected;
    int helpCheck = 0;
    int count = 20000;
    int helpCheckTwo = 0;
    public BookDownload()
    {
        InitializeComponent();
        try
        {
            BookDownloadIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
        CallBaza();
        foreach (Series s in ListsStaticClass.listAllSeries)
        {
            if (s.UserId == ListsStaticClass.currentAccount)
            {
                seriesList.Add(s);
            }
        }
        boxForSeries.ItemsSource = seriesList.ToList();
    }
    private async void AddNewPhoto(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        helpCheckTwo = 1;
        var dialog = new OpenFileDialog
        {
            AllowMultiple = false,
            Filters = new List<FileDialogFilter>
        {
            new FileDialogFilter { Name = "Images", Extensions = { "png", "jpg", "jpeg", "bmp" } }
        }
        };

        var result = await dialog.ShowAsync(this);

        if (result != null && result.Length > 0)
        {
            var sourcePath = result[0];
            var extension = Path.GetExtension(sourcePath);
            var fileName = $"bookcover_{Guid.NewGuid()}{extension}";

            var destDir = Path.Combine(AppContext.BaseDirectory, "PhotoForBook");
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);

            var destPath = Path.Combine(destDir, fileName);
            File.Copy(sourcePath, destPath, true);

            selectedImageFileName = fileName;

            using (var stream = File.OpenRead(destPath))
            {
                var bitmap = new Avalonia.Media.Imaging.Bitmap(stream);
                coverPreview.Source = bitmap;
            }
        }
    }
    private void AddNewAppBook(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(newTitle.Text) && !string.IsNullOrEmpty(newDescription.Text) && !string.IsNullOrEmpty(newSinopsis.Text))
        {
            if (helpCheckTwo == 0)
            {
                selectedImageFileName = "PhotoCheck.png.png"; // Имя файла по умолчанию
            }
            ListsStaticClass.listAllBooks = ListsStaticClass.listAllBooks.OrderBy(x => x.Id).ToList();
            int newBookId = ListsStaticClass.listAllBooks.LastOrDefault().Id + 1;
            // Здесь идёт добавление книги в БД с обложкой
            if (helpCheck == 1)
            {
                var newBook = new Book
                {
                    Id = ListsStaticClass.listAllBooks.LastOrDefault().Id + 1,
                    Title = newTitle.Text.Trim(),
                    Description = newDescription.Text.Trim(),
                    Synopsis = newSinopsis.Text.Trim(),
                    CoverImage = selectedImageFileName,
                    Rating = 0,
                    Kolplan = 0,
                    Kolread = 0,
                    Kolrev = 0,
                    Dateadd = DateTime.Now,
                    IsAuthorBook = true,
                    SeriesId = selected,
                };
                Baza.DbContext.Books.Add(newBook);
                Baza.DbContext.SaveChanges();
            }
            else
            {
                var newBook = new Book
                {
                    Id = ListsStaticClass.listAllBooks.LastOrDefault().Id + 1,
                    Title = newTitle.Text.Trim(),
                    Description = newDescription.Text.Trim(),
                    Synopsis = newSinopsis.Text.Trim(),
                    CoverImage = selectedImageFileName,
                    Rating = 0,
                    Kolplan = 0,
                    Kolread = 0,
                    Kolrev = 0,
                    Dateadd = DateTime.Now,
                    IsAuthorBook = true,
                };
                Baza.DbContext.Books.Add(newBook);
                Baza.DbContext.SaveChanges();
            }
            ListsStaticClass.listAllBookAuthors = ListsStaticClass.listAllBookAuthors.OrderBy(x => x.Id).ToList();
            var newAuthorBook = new Bookauthor
            {
                Id = ListsStaticClass.listAllBookAuthors.LastOrDefault().Id + 1,
                AppAuthorId = ListsStaticClass.currentAccount,
                BookId = newBookId,
            };
            Baza.DbContext.Bookauthors.Add(newAuthorBook);
            Baza.DbContext.SaveChanges();

            int newElectronicBookId = (Baza.DbContext.ElectronicBooksInfos.Any()
                     ? Baza.DbContext.ElectronicBooksInfos.Max(e => e.Id)
                     : 0) + 1;
            var newAppBook = new ElectronicBooksInfo
            {
                Id = newElectronicBookId,
                BookId = newBookId,
                TargetWordCount = count,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                AccessLevelId = 2,
                StatusId = 1,
            };
            Baza.DbContext.ElectronicBooksInfos.Add(newAppBook);
            Baza.DbContext.SaveChanges();

            new AuthorBoard().Show();
            Close();
        }
        else
        {
            error.IsVisible = true;
            error.Text = "Не все поля заполнены!";
        }
    }
    private void ComboBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        var comboBox = sender as ComboBox;
        if (comboBox?.SelectedItem is Series selectedSeries)
        {
            helpCheck = 1;
            selected = selectedSeries.Id;
        }
    }
    private async void OpenNewSeriesAdd(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var newSeriesWindow = new NewAddSeries();
        await newSeriesWindow.ShowDialog(this);

        if (newSeriesWindow.SeriesAdded)
        {
            seriesList.Clear();
            boxForSeries.ItemsSource = seriesList.ToList();
            CallBaza();
            foreach (Series s in ListsStaticClass.listAllSeries)
            {
                if (s.UserId == ListsStaticClass.currentAccount)
                {
                    seriesList.Add(s);
                }
            }
            boxForSeries.ItemsSource = seriesList.ToList();
        }
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
    private void CallBaza()
    {
        ListsStaticClass.listAllSeries.Clear();
        ListsStaticClass.listAllSeries = Baza.DbContext.Series.Select(series => new Series
        {
            Id = series.Id,
            Title = series.Title,
            UserId = series.UserId,
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
        ListsStaticClass.listAllBookAuthors.Clear();
        ListsStaticClass.listAllBookAuthors = Baza.DbContext.Bookauthors.Select(book => new Bookauthor
        {
            Id = book.Id,
            AuthorId = book.AuthorId,
            AppAuthorId = book.AppAuthorId,
            BookId = book.BookId,
        }).ToList();
    }

    private void MicroOpen(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        sizeText.Text = "500";
        count = 500;
    }
    private void SmallOpen(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        sizeText.Text = "5 000";
        count = 5000;
    }
    private void MiddleOpen(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        sizeText.Text = "20 000";
        count = 20000;
    }
    private void LongOpen(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        sizeText.Text = "50 000";
        count = 50000;
    }
    private void VeryLongOpen(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        sizeText.Text = "от 50 000";
        count = 60000;
    }

    private void FreeMode(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        sizeText.Text = " ";
        count = 0;
    }

    private void Back(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new AuthorBoard().Show();
        Close();
    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Friends().Show();
        Close();
    }
}