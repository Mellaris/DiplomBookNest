using Avalonia.Controls;
using Avalonia.Media.Imaging;
using DiplomTwo.Models;
using System;
using System.Linq;

namespace DiplomTwo;

public partial class NewAddSeries : Window
{
    public bool SeriesAdded { get; private set; } = false;
    public NewAddSeries()
    {
        InitializeComponent();
        try
        {
            NewAddSeriesIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
        CallBaza();
    }

    private void Back(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close();
    }

    private async void Add(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        string newSeriesTittle = newSeries.Text;
        if (!string.IsNullOrEmpty(newSeriesTittle))
        {
            int currentId = ListsStaticClass.currentAccount;
            bool seriesExists = Baza.DbContext.Series.Any(s => s.Title == newSeriesTittle && s.UserId == currentId);
            if (seriesExists)
            {
                errorMassege.IsVisible = true;
                errorMassege.Text = "Ошибка, у вас уже есть серия с таким названием";
            }
            else
            {
                ListsStaticClass.listAllSeries = ListsStaticClass.listAllSeries.OrderBy(user => user.Id).ToList();
                var seriesNew = new Series
                {
                    Id = ListsStaticClass.listAllSeries.LastOrDefault().Id + 1,
                    Title = newSeriesTittle,
                    UserId = currentId,
                };
                Baza.DbContext.Series.Add(seriesNew);
                Baza.DbContext.SaveChanges();

                SeriesAdded = true;

                this.Close();
            }
        }
        else
        {
            errorMassege.IsVisible = true;
            errorMassege.Text = "Ошибка, Название серии не может быть пустым";
        }     
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
}