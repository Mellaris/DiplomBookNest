using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DiplomTwo;

public partial class ErrorReport : Window
{
    public ErrorReport()
    {
        InitializeComponent();
    }
    public ErrorReport(string errorThis)
    {
        InitializeComponent();
        error.Text = errorThis;
    }
}