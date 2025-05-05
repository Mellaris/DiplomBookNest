using Avalonia.Controls;
using System.Linq;
using DiplomTwo.Models;
using DiplomTwo.Context;
using System.IO;
using System.Collections.Generic;
using Avalonia.Media.Imaging;
using System;
using static System.Reflection.Metadata.BlobBuilder;

namespace DiplomTwo
{
    public partial class MainWindow : Window
    {
        private List<Book> booksWithTop = new List<Book>();
       
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                Main.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
            }
            catch { }
            CallBaza();
            SortForTopList();
            ListForGenre();
        }
        private void ListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
        {
            var selectedBook = topFiveBooks.SelectedItem as Book;

            if (selectedBook != null)
            {
                int bookId = selectedBook.Id; // Получаем id книги
                new SpecificBook(bookId).Show();
                Close();
            }
        }
        private void SearchBook(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var allBooks = Baza.DbContext.Books.AsQueryable();

            // Жанр
            if (genreForBooks.SelectedItem is Genre selectedGenre)
            {
                var bookIdsByGenre = Baza.DbContext.BookGenres
                    .Where(bg => bg.GenreId == selectedGenre.Id)
                    .Select(bg => bg.BookId)
                    .ToList();

                allBooks = allBooks.Where(b => bookIdsByGenre.Contains(b.Id));
            }

            // Возраст
            if (ageForBook.SelectedItem != null)
            {
                if (int.TryParse(ageForBook.SelectedItem.ToString(), out int age))
                {
                    allBooks = allBooks.Where(b => b.AgeLimit == age);
                }
            }

            // Кол-во страниц
            if (pagesForBook.SelectedItem is string pages)
            {
                switch (pages)
                {
                    case "До 300":
                        allBooks = allBooks.Where(b => b.PageCount <= 300);
                        break;
                    case "От 301 до 500":
                        allBooks = allBooks.Where(b => b.PageCount >= 301 && b.PageCount <= 500);
                        break;
                    case "От 501":
                        allBooks = allBooks.Where(b => b.PageCount > 500);
                        break;
                }
            }

            // Рейтинг
            if (ratingForBook.SelectedItem is string ratingText)
            {
                int minRating = 0;
                if (ratingText.Contains("+"))
                    int.TryParse(ratingText.Replace("+", ""), out minRating);
                else
                    int.TryParse(ratingText, out minRating);

                allBooks = allBooks.Where(b => b.Rating >= minRating);
            }

            // Получаем подходящие книги
            var matchingBooks = allBooks.ToList();

            if (matchingBooks.Any())
            {
                var random = new Random();
                var selectedBook = matchingBooks[random.Next(matchingBooks.Count)];

                // Покажи обложку — замените на свою реализацию
                bookCoverImage.Source = (selectedBook.CoverBitmap);
            }
            else
            {
                new ErrorReport("Не найдено ни одной книги по выбранным фильтрам.").ShowDialog(this);
            }
        }
        private void ListForGenre()
        {
            ListsStaticClass.listAllGenres.Clear();
            ListsStaticClass.listAllGenres = Baza.DbContext.Genres.Select(genre => new Genre
            {
                Id = genre.Id,
                Name = genre.Name,
            }).ToList();

            genreForBooks.ItemsSource = ListsStaticClass.listAllGenres;
        }

        public void CallBaza()
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
            }).ToList();

            ListsStaticClass.listAllBookGenres.Clear();
            ListsStaticClass.listAllBookGenres = Baza.DbContext.BookGenres.Select(bookGenre => new BookGenre
            {
                Id = bookGenre.Id,
                BookId = bookGenre.BookId,
                GenreId = bookGenre.GenreId,
            }).ToList();
        }
        private void SortForTopList()
        {
            booksWithTop.Clear();
            booksWithTop = ListsStaticClass.listAllBooks.OrderByDescending(book => book.Rating).Take(10).ToList();
            topFiveBooks.ItemsSource = booksWithTop.ToList();
        }
        private void MyAccount(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            new personalAccount().Show();
            Close();
        }

        private void Log(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if(ListsStaticClass.currentAccount == -1)
            {
                new LogIn().Show();
                Close();
            }
            else
            {
                new personalAccount().Show();
                Close();
            }
        }

        private void AllBooks(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            new Books().Show();
            Close();
        }

        private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            new Friends().Show();
            Close();
        }

        
    }

}