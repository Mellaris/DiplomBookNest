using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DiplomTwo.Models;
using System.Linq;

namespace DiplomTwo;

public partial class MessageCongratulations : Window
{
    public MessageCongratulations()
    {
        InitializeComponent();
    }
    public MessageCongratulations(int id)
    {
        InitializeComponent();
        ListsStaticClass.listAllAchievement = Baza.DbContext.Achievements.ToList();
        foreach(Achievement achievement in ListsStaticClass.listAllAchievement)
        {
            if(achievement.Id == id)
            {
                imageForA.Source = achievement.CoverBitmap;
                nameA.Text = achievement.Name;
                break;
            }
        }
    }
    
}