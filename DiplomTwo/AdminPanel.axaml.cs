using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DiplomTwo;

public partial class AdminPanel : Window
{
    public AdminPanel()
    {
        InitializeComponent();
    }

    private void LigInBack(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new LogIn().Show();
        Close();
    }

    private void OpenMod(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Moderation().Show();
        Close();
    }
}