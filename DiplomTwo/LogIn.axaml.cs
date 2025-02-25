using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DiplomTwo;

public partial class LogIn : Window
{
    public LogIn()
    {
        InitializeComponent();
    }

    private void RegistrNewAcc(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Registration().Show();
        Close();
    }
}