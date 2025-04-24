using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;

namespace DiplomTwo;

public partial class SpecificBook : Window
{
    public SpecificBook()
    {
        InitializeComponent();
        try
        {
            SpecificBookIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
    }
}