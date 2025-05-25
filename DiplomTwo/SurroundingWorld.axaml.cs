using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Linq;
using DiplomTwo.Models;
using Tmds.DBus.Protocol;
using static System.Collections.Specialized.BitVector32;

namespace DiplomTwo;

public partial class SurroundingWorld : Window
{
    private int idThis;
    private int idSection;
    private int Check = 0;
    public SurroundingWorld()
    {
        InitializeComponent();
    }
    public SurroundingWorld(int id)
    {
        InitializeComponent();
        CallBaza();
        idThis = id;
        ListsStaticClass.listAllWorldSection = ListsStaticClass.listAllWorldSection.OrderBy(x => x.Id).ToList();
        AllSections.ItemsSource = ListsStaticClass.listAllWorldSection.ToList();
    }
    private void CallBaza()
    {
        ListsStaticClass.listAllWorldSection.Clear();
        ListsStaticClass.listAllWorldSection = Baza.DbContext.WorldSections.Select(world => new WorldSection
        {
            Id = world.Id,
            Name = world.Name,
            ShortDescription = world.ShortDescription
        }).ToList();

        ListsStaticClass.listAllWorldsectionContent.Clear();
        ListsStaticClass.listAllWorldsectionContent = Baza.DbContext.WorldSectionContents.Select(worldContent => new WorldSectionContent
        {
            Id = worldContent.Id,
            BookId = worldContent.BookId,
            WorldSectionId = worldContent.WorldSectionId,
            Content = worldContent.Content,
            CreatedAt = worldContent.CreatedAt,
            UpdatedAt = worldContent.UpdatedAt,
        }).ToList();
    }

    private void ListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        if (AllSections.SelectedItem != null)
        {
            var selectedSections = AllSections.SelectedItem as WorldSection;
            if (selectedSections != null)
            {
                idSection = selectedSections.Id;
                OpenNewSection(idSection);
            }
        }
    }
    private void OpenNewSection(int idSectThis)
    {
        foreach(WorldSectionContent worldSectionContent in ListsStaticClass.listAllWorldsectionContent)
        {
            if(worldSectionContent.BookId == idThis && worldSectionContent.WorldSectionId == idSection)
            {
                var id = Baza.DbContext.WorldSections.FirstOrDefault(u => u.Id == idSection);
                titleSection.Text = id.Name;
                contentSection.Text = worldSectionContent.Content;
                break;
            }
            else
            {
                foreach (WorldSection section in ListsStaticClass.listAllWorldSection)
                {
                    if (section.Id == idSectThis)
                    {
                        titleSection.Text = section.Name;
                        contentSection.Text = section.ShortDescription;
                        break;
                    }
                }
            }
        }
       
    }
    private void SaveSection(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        //Обновление
        foreach(WorldSectionContent worldSectionContent in ListsStaticClass.listAllWorldsectionContent)
        {
            if(worldSectionContent.BookId == idThis && worldSectionContent.WorldSectionId == idSection)
            {
                Check = 1;
                var id = Baza.DbContext.WorldSectionContents.FirstOrDefault(u => u.BookId == idThis && u.WorldSectionId == idSection);
                if (id != null)
                {
                    if (!string.IsNullOrEmpty(titleSection.Text))
                    {
                        id.Content = contentSection.Text;
                        id.UpdatedAt = System.DateTime.Now;
                        Baza.DbContext.SaveChanges();
                    }
                    
                }
                CallBaza();
               break;
            }
        }
        //Добавление
        if(Check == 0)
        {
            if(!string.IsNullOrEmpty(titleSection.Text))
            {
                var addContent = new WorldSectionContent
                {
                    WorldSectionId = idSection,
                    BookId = idThis,
                    Content = contentSection.Text,
                    CreatedAt = System.DateTime.Now,
                    UpdatedAt = System.DateTime.Now,
                };
                Baza.DbContext.WorldSectionContents.Add(addContent);
                Baza.DbContext.SaveChanges();
            }
            
            CallBaza();
        }
        Check = 0;
    }

    private void Home(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }

    private void OpenB(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Books().Show();
        Close();
    }

    private void My(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new personalAccount().Show();
        Close();
    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Friends().Show();
        Close();
    }

    private void Button_Click_2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new CreatingBook(idThis).Show();
        Close();
    }
}