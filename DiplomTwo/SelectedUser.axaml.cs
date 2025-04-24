using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;

namespace DiplomTwo;

public partial class SelectedUser : Window
{
    public SelectedUser()
    {
        InitializeComponent();
        try
        {
            SelectedUserIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
    }
}