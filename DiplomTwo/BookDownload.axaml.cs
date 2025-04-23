using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DiplomTwo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DiplomTwo;

public partial class BookDownload : Window
{
    private List<Series> seriesList = new List<Series>();
    private string selectedImageFileName = null;
    public BookDownload()
    {
        InitializeComponent();
        CallBaza();
        foreach(Series s in ListsStaticClass.listAllSeries)
        {
            if(s.UserId == ListsStaticClass.currentAccount)
            {
                seriesList.Add(s);
            }
        }
        boxForSeries.ItemsSource = seriesList.ToList();
    }
    private async void AddNewPhoto(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            AllowMultiple = false,
            Filters = new List<FileDialogFilter>
        {
            new FileDialogFilter { Name = "Images", Extensions = { "png", "jpg", "jpeg", "bmp" } }
        }
        };

        var result = await dialog.ShowAsync(this);

        if (result != null && result.Length > 0)
        {
            var sourcePath = result[0];
            var extension = Path.GetExtension(sourcePath);
            var fileName = $"bookcover_{Guid.NewGuid()}{extension}";

            var destDir = Path.Combine(AppContext.BaseDirectory, "PhotoForBook");
            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);

            var destPath = Path.Combine(destDir, fileName);
            File.Copy(sourcePath, destPath, true);

            selectedImageFileName = fileName;

            using (var stream = File.OpenRead(destPath))
            {
                var bitmap = new Avalonia.Media.Imaging.Bitmap(stream);
                coverPreview.Source = bitmap;
            }
        }
    }
    private void AddNewAppBook(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        
    }
    private void OpenNewSeriesAdd(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new NewAddSeries().ShowDialog(this);
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

    private void AllBooks(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Books().Show();
        Close();
    }
    private void CallBaza()
    {
        ListsStaticClass.listAllSeries.Clear();
        ListsStaticClass.listAllSeries = Baza.DbContext.Series.Select(series => new Series
        {
            Id = series.Id,
            Title = series.Title,
            UserId = series.UserId,
        }).ToList();
    }

    private void MicroOpen(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        sizeText.Text = "500";
    }
    private void SmallOpen(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        sizeText.Text = "5 000";
    }
    private void MiddleOpen(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        sizeText.Text = "20 000";
    }
    private void LongOpen(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        sizeText.Text = "50 000";
    }
    private void VeryLongOpen(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        sizeText.Text = "от 50 000";
    }

    private void FreeMode(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        sizeText.Text = " ";
    }
}