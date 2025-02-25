using Avalonia.Controls;

namespace DiplomTwo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MyAccount(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            new personalAccount().Show();
            Close();
        }

        private void Log(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            new LogIn().Show();
            Close();
        }
    }
}