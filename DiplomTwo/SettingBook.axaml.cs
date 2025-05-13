using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;
using System.Linq;
using DiplomTwo.Models;
using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using DiplomTwo.Context;

namespace DiplomTwo;

public partial class SettingBook : Window
{
    int helpCheckTwo = 0;
    private int selectIdLevel;
    private int selectIdStatus;
    private int selectIdGenre = -1;
    private string selectedImageFileName = null;
    private int currentBookId;
    public SettingBook()
    {
        InitializeComponent();
    }
    public SettingBook(int id)
    {
        InitializeComponent();
        try
        {
            SettingBookIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
        currentBookId = id;
        CallBaza();
        foreach (Book book in ListsStaticClass.listAllBooks)
        {
            if (book.Id == id)
            {
                imageBook.Source = book.CoverBitmap;
                ageBook.Text = book.AgeLimit.ToString();
                nameBook.Text = book.Title;
                descriptBook.Text = book.Description;
                sinopsisBook.Text = book.Synopsis;
                qBook.Text = book.Quote;
                break;
            }
        }
        foreach (ElectronicBooksInfo electronicBooksInfo in ListsStaticClass.listAllElectronicBooksInfo)
        {
            if (electronicBooksInfo.BookId == id)
            {
                selectIdLevel = Convert.ToInt32(electronicBooksInfo.AccessLevelId);
                selectIdStatus = Convert.ToInt32(electronicBooksInfo.StatusId);
                break;
            }
        }
        nameStatus.ItemsSource = ListsStaticClass.listAllWritingStatus.OrderBy(x => x.Id).ToList();
        nameLevel.ItemsSource = ListsStaticClass.listAllPublicaccesslevel.OrderBy(x => x.Id).ToList();
        genreName.ItemsSource = ListsStaticClass.listAllGenres.OrderBy(x => x.Id).ToList();

        nameStatus.SelectedIndex = selectIdStatus - 1;
        nameLevel.SelectedIndex = selectIdLevel - 1;

        foreach (BookGenre bookGenre in ListsStaticClass.listAllBookGenres)
        {
            if (bookGenre.BookId == id)
            {
                foreach (Genre genre in ListsStaticClass.listAllGenres)
                {
                    if (genre.Id == bookGenre.GenreId)
                    {
                        selectIdGenre = Convert.ToInt32(genre.Id);
                        break;
                    }
                }
                break;
            }
        }
        if (selectIdGenre > -1)
        {
            genreName.SelectedIndex = selectIdGenre - 1;
        }
    }
    private void SaveAndBack(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        using var db = new User1Context();

        // Загружаем книгу из базы данных
        var book = db.Books.FirstOrDefault(b => b.Id == currentBookId);
        if (book != null)
        {
            book.Title = nameBook.Text;
            book.AgeLimit = int.TryParse(ageBook.Text, out int age) ? age : 0;
            book.Description = descriptBook.Text;
            book.Synopsis = sinopsisBook.Text;
            book.Quote = qBook.Text;

            if (!string.IsNullOrEmpty(selectedImageFileName))
            {
                book.CoverImage = selectedImageFileName;
            }
        }

        // Загружаем ElectronicBooksInfo
        var info = db.ElectronicBooksInfos.FirstOrDefault(e => e.BookId == currentBookId);
        if (info != null)
        {
            if (nameStatus.SelectedItem is WritingStatus status)
                info.StatusId = status.Id;

            if (nameLevel.SelectedItem is Publicaccesslevel level)
                info.AccessLevelId = level.Id;
        }

        // Обновляем жанр
        ListsStaticClass.listAllBookGenres = ListsStaticClass.listAllBookGenres.OrderBy(x => x.Id).ToList();
        var bookGenre = db.BookGenres.FirstOrDefault(bg => bg.BookId == currentBookId);
        if (genreName.SelectedItem is Genre selectedGenre)
        {
            if (bookGenre != null)
            {
                bookGenre.GenreId = selectedGenre.Id;
            }
            else
            {
                var newGenreBook = new BookGenre
                {
                    Id = ListsStaticClass.listAllBookGenres.LastOrDefault().Id + 1,
                    BookId = currentBookId,
                    GenreId = selectedGenre.Id
                };
                db.BookGenres.Add(newGenreBook);
            }
        }

        // Сохраняем всё одним вызовом
        db.SaveChanges();
        // Обновляем
        CallBaza();
    }


    private async void EditPhotoBook(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
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
                imageBook.Source = bitmap;
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
            Quote = book.Quote,
            BookGenres = book.BookGenres.ToList(),
        }).ToList();

        ListsStaticClass.listAllUsers.Clear();
        ListsStaticClass.listAllUsers = Baza.DbContext.Users.Select(user => new User
        {
            Id = user.Id,
            Username = user.Username,
            Userlastname = user.Userlastname,
            Email = user.Email,
            Passwords = user.Passwords,
            Login = user.Login,
            RoleId = user.RoleId,
            GenderId = user.GenderId,
            RegistrationDate = user.RegistrationDate,
        }).ToList();

        ListsStaticClass.listAllReaders.Clear();
        ListsStaticClass.listAllReaders = Baza.DbContext.Readers.Select(reader => new Reader
        {
            IdReader = reader.IdReader,
            UserId = reader.UserId,
            YearlyGoal = reader.YearlyGoal,
            AvatarUrl = reader.AvatarUrl,
            ProfileDescription = reader.ProfileDescription,
        }).ToList();

        ListsStaticClass.listAllPersonallLibrary.Clear();
        ListsStaticClass.listAllPersonallLibrary = Baza.DbContext.Personallibraries.Select(library => new Personallibrary
        {
            Id = library.Id,
            ReaderId = library.ReaderId,
            BookId = library.BookId,
            Rating = library.Rating,
            PlotRating = library.PlotRating,
            CharactersRating = library.CharactersRating,
            WorldRating = library.WorldRating,
            RomanceRating = library.RomanceRating,
            HumorRating = library.HumorRating,
            MeaningRating = library.MeaningRating,
            DateAdd = library.DateAdd,
            Feedback = library.Feedback,
        }).ToList();

        ListsStaticClass.listAllBookAuthors.Clear();
        ListsStaticClass.listAllBookAuthors = Baza.DbContext.Bookauthors.Select(ba => new Bookauthor
        {
            Id = ba.Id,
            AuthorId = ba.AuthorId,
            AppAuthorId = ba.AppAuthorId,
            BookId = ba.BookId,
        }).ToList();

        ListsStaticClass.listAllAuthors.Clear();
        ListsStaticClass.listAllAuthors = Baza.DbContext.Authors.Select(authors => new Author
        {
            Id = authors.Id,
            Name = authors.Name,
        }).ToList();

        ListsStaticClass.listAllAppAuthors.Clear();
        ListsStaticClass.listAllAppAuthors = Baza.DbContext.Appauthors.Select(appA => new Appauthor
        {
            Id = appA.Id,
            RegistrationDate = appA.RegistrationDate,
        }).ToList();

        ListsStaticClass.listAllQuote.Clear();
        ListsStaticClass.listAllQuote = Baza.DbContext.Quotes.Select(q => new Quote
        {
            Id = q.Id,
            BookId = q.BookId,
            ReaderId = q.ReaderId,
            Text = q.Text,
        }).ToList();

        ListsStaticClass.listAllBookreview.Clear();
        ListsStaticClass.listAllBookreview = Baza.DbContext.Bookreviews.Select(br => new Bookreview
        {
            Id = br.Id,
            BookId = br.BookId,
            ReaderId = br.ReaderId,
            ReviewText = br.ReviewText,
        }).ToList();

        ListsStaticClass.listAllWritingStatus.Clear();
        ListsStaticClass.listAllWritingStatus = Baza.DbContext.WritingStatuses.Select(wrSt => new WritingStatus
        {
            Id = wrSt.Id,
            Name = wrSt.Name,
        }).ToList();

        ListsStaticClass.listAllPublicaccesslevel.Clear();
        ListsStaticClass.listAllPublicaccesslevel = Baza.DbContext.Publicaccesslevels.Select(pb => new Publicaccesslevel
        {
            Id = pb.Id,
            Name = pb.Name,
        }).ToList();

        ListsStaticClass.listAllElectronicBooksInfo.Clear();
        ListsStaticClass.listAllElectronicBooksInfo = Baza.DbContext.ElectronicBooksInfos.Select(electronic => new ElectronicBooksInfo
        {
            Id = electronic.Id,
            BookId = electronic.BookId,
            StatusId = electronic.StatusId,
            AccessLevelId = electronic.AccessLevelId,
        }).ToList();

        ListsStaticClass.listAllGenres.Clear();
        ListsStaticClass.listAllGenres = Baza.DbContext.Genres.Select(pb => new Genre
        {
            Id = pb.Id,
            Name = pb.Name,
        }).ToList();

        ListsStaticClass.listAllBookGenres.Clear();
        ListsStaticClass.listAllBookGenres = Baza.DbContext.BookGenres.Select(pb => new BookGenre
        {
            Id = pb.Id,
            BookId = pb.BookId,
            GenreId = pb.GenreId,
        }).ToList();
    }

    private void DeleteThisBook(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        using var db = new User1Context();

        var book = db.Books.FirstOrDefault(x => x.Id == currentBookId);
        if (book == null)
            return;

        // Удаляем связанные записи
        var infos = db.ElectronicBooksInfos.Where(i => i.BookId == currentBookId);
        db.ElectronicBooksInfos.RemoveRange(infos);

        var genres = db.BookGenres.Where(bg => bg.BookId == currentBookId);
        db.BookGenres.RemoveRange(genres);

        var personalLibs = db.Personallibraries.Where(pl => pl.BookId == currentBookId);
        db.Personallibraries.RemoveRange(personalLibs);

        var reviews = db.Bookreviews.Where(r => r.BookId == currentBookId);
        db.Bookreviews.RemoveRange(reviews);

        var quotes = db.Quotes.Where(q => q.BookId == currentBookId);
        db.Quotes.RemoveRange(quotes);

        var plan = db.Bookplans.Where(pl => pl.BookId == currentBookId);
        db.Bookplans.RemoveRange(plan);

        //var comments = db.Comments.Where(c => c. == currentBookId);
        //db.Comments.RemoveRange(comments);

        // Удаляем книгу
        db.Books.Remove(book);
        db.SaveChanges();

        CallBaza(); // Обновить локальные списки
        new AuthorBoard().Show();
        Close();
    }

    private void OpenBack(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new personalAccount().Show();
        Close();
    }

    private void Home(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }

    private void AllBooks(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Books().Show();
        Close();
    }
}