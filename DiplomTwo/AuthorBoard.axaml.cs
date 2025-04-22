using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DiplomTwo;

public partial class AuthorBoard : Window
{
    public AuthorBoard()
    {
        InitializeComponent();
    }
    private void AddNewBook(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new BookDownload().Show();
        Close();
    }

    private void BackAccount(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new personalAccount().Show();
        Close();
    }

    private void Home(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }

    private void BooksAll(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Books().Show();
        Close();
    }

    private void OpenAuthors(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new AllAuthors().Show();
        Close();
    }

    private void OpenNews(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Headline().Show();
        Close();
    }

   
}