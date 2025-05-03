using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DiplomTwo;

public partial class CharacterСreation : Window
{
    public CharacterСreation()
    {
        InitializeComponent();
    }

    private void Home(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }

    private void OpenBooks(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Books().Show();
        Close();
    }

    private void MyAccount(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new personalAccount().Show();
        Close();
    }
}