using Avalonia.Controls;
using System.Linq;
using DiplomTwo.Models;
using DiplomTwo.Context;
using System.IO;
using System.Collections.Generic;

namespace DiplomTwo
{
    public partial class MainWindow : Window
    {
        private List<Book> booksWithTop = new List<Book>();
        public MainWindow()
        {
            InitializeComponent();
            CallBaza();
            SortForTopList();
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
            new AllAuthors().Show();
            Close();
        }
    }
}