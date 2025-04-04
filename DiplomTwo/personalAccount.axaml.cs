using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DiplomTwo;

public partial class personalAccount : Window
{
    public personalAccount()
    {
        InitializeComponent();
    }
    private void Log(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new LogIn().Show();
        Close();
    }

    private void MyLibraryBooks(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MyBooks().Show();
        Close();
    }

    private void Home(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }
}