using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;

namespace DiplomTwo;

public partial class WritingReview : Window
{
    public WritingReview()
    {
        InitializeComponent();
        try
        {
            WritingReviewIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
    }
}