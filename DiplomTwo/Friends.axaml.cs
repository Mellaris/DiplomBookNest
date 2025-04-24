using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;

namespace DiplomTwo;

public partial class Friends : Window
{
    public Friends()
    {
        InitializeComponent();
        try
        {
            FriendsIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
    }
}