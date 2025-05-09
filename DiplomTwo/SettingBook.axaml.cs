using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;

namespace DiplomTwo;

public partial class SettingBook : Window
{
    public SettingBook()
    {
        InitializeComponent();
    }
    public SettingBook(int id)
    {
        InitializeComponent();
        try
        {
            SettingBookIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
    }
}