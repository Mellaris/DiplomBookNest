using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Linq;
using DiplomTwo.Models;

namespace DiplomTwo;

public partial class AddingQuotes : Window
{
    private int idThis;
    public AddingQuotes()
    {
        InitializeComponent();
    }
    public AddingQuotes(int id)
    {
        InitializeComponent();
        CallBaza();
        idThis = id;
    }
    private void SafeThis(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if(!string.IsNullOrEmpty(textForAdd.Text))
        {
            try
            {
                var addQ = new Quote
                {
                    BookId = idThis,
                    ReaderId = ListsStaticClass.currentAccount,
                    Text = textForAdd.Text,
                };
                Baza.DbContext.Quotes.Add(addQ);
                Baza.DbContext.SaveChanges();
                textError.IsVisible = true;
                textError.Text = "Успешно!";
            }
            catch
            {
                textError.IsVisible = true;
                textError.Text = "Ощибка!";
            }
            
        }
        else
        {
            textError.Text = "Поле не может быть пустым!";
            textError.IsVisible = true;
        }
    }
    private void CallBaza()
    {
        ListsStaticClass.listAllQuote.Clear();
        ListsStaticClass.listAllQuote = Baza.DbContext.Quotes.Select(quotes => new Quote
        {
            Id = quotes.Id,
            BookId = quotes.BookId,
            ReaderId = quotes.ReaderId,
            Text = quotes.Text,
        }).ToList();

    }
}