using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;

namespace DiplomTwo;

public partial class Headline : Window
{
    public Headline()
    {
        InitializeComponent();
        try
        {
            HeadlineIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
    }
}