using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Linq;
using DiplomTwo.Models;
using System;

namespace DiplomTwo;

public partial class AddingQuotes : Window
{
    private int idThis;
    private WritingReview parentWindow;
    public AddingQuotes()
    {
        InitializeComponent();
    }
    public AddingQuotes(int id, WritingReview parent)
    {
        InitializeComponent();
        CallBaza();
        idThis = id;
        parentWindow = parent;
    }
    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
        parentWindow.RefreshQuotesList();
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

                int currentUserId = ListsStaticClass.currentAccount;

                // Проверка: есть ли уже достижение с ID = 3
                bool alreadyHasAchievement = Baza.DbContext.Userachievements
                    .Any(ua => ua.ReaderId == currentUserId && ua.AchievementId == 3);

                // Считаем количество цитат пользователя
                int quotesCount = Baza.DbContext.Quotes
                    .Count(q => q.ReaderId == currentUserId); // замените UserId на нужное имя поля, если оно другое

                if (!alreadyHasAchievement && quotesCount >= 9)
                {
                    var newAch = new Userachievement
                    {
                        ReaderId = currentUserId,
                        AchievementId = 3,
                        EarnedAt = DateTime.Now,
                    };
                    Baza.DbContext.Userachievements.Add(newAch);
                    Baza.DbContext.SaveChanges();

                    int ach = 3;
                    new MessageCongratulations(ach).ShowDialog(this);
                }

            }
            catch
            {
                textError.IsVisible = true;
                textError.Text = "Ошибка!";
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