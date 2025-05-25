using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using System;
using System.Linq;
using DiplomTwo.Models;
using System.Collections.Generic;
using System.IO;

namespace DiplomTwo;

public partial class CharacterСreation : Window
{
    private int idThisBook;
    private List<Character> characterList = new List<Character>();
    private int charecterIdThisBook;
    private string? selectedPhotoFilePath; // Полный путь к выбранному файлу
    private string? photoFileName;
    private int idС;
    public CharacterСreation()
    {
        InitializeComponent();
    }
    public CharacterСreation(int id)
    {
        InitializeComponent();
        try
        {
            CharacterСreationIcon.Icon = new WindowIcon(new Bitmap(Environment.CurrentDirectory + "/" + "icon.ico"));
        }
        catch { }
        idThisBook = id;
        CallBaza();
        foreach(Character character in ListsStaticClass.listAllCharacter)
        {
            if(character.BookId == idThisBook)
            {
                characterList.Add(character);
            }
        }
        allCharacter.ItemsSource = characterList.ToList();
    }
    private void AddChar(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var newChar = new Character
        {
            BookId = idThisBook,
            Name = "Новый персонаж",
            GenderId = 3,
            ImageName = "photoCheck.jpeg",
        };
        Baza.DbContext.Characters.Add(newChar);
        Baza.DbContext.SaveChanges();

        // Обновляем локальный список и перерисовываем лист
        ListsStaticClass.listAllCharacter.Add(newChar);
        characterList.Add(newChar);
        allCharacter.ItemsSource = null;
        allCharacter.ItemsSource = characterList.ToList();
    }
    private void Home(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new MainWindow().Show();
        Close();
    }

    private void OpenBooks(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Books().Show();
        Close();
    }

    private void MyAccount(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new personalAccount().Show();
        Close();
    }
    private void SaveNew(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var character = Baza.DbContext.Characters.FirstOrDefault(c => c.Id == idС); // Замените currentCharacterId на ваш актуальный id

        if (character != null)
        {
            // Обновляем поля
            character.Name = titleCharacter.Text;
            character.Personality = persona.Text;
            character.Hobbies = hobby.Text;
            character.Goal = goal.Text;
            character.Ethnicity = ens.Text;
            character.Fears = fear.Text;
            character.Weaknesses = minus.Text;
            character.Strengths = plus.Text;
            character.Occupation = study.Text;
            character.Description = descr.Text;

            if (int.TryParse(age.Text, out int parsedAge))
            {
                character.Age = parsedAge;
            }

            // Обновим фото, если имя файла изменилось
            if (!string.IsNullOrEmpty(photoFileName))
            {
                character.ImageName = photoFileName;

                // Если выбрано новое фото (а не дефолтное)
                if (selectedPhotoFilePath != null)
                {
                    string targetFolder = Path.Combine(AppContext.BaseDirectory, "PhotosForCharacters");
                    string targetPath = Path.Combine(targetFolder, photoFileName);

                    if (!File.Exists(targetPath))
                    {
                        File.Copy(selectedPhotoFilePath, targetPath);
                    }
                }
            }

            // Сохраняем изменения
            Baza.DbContext.SaveChanges();

            CallBaza();
            allCharacter.ItemsSource = null;
            characterList.Clear();
            foreach (Character characterr in ListsStaticClass.listAllCharacter)
            {
                if (characterr.BookId == idThisBook)
                {
                    characterList.Add(characterr);
                }
            }
            allCharacter.ItemsSource = characterList.ToList();

        }
        else
        {
            new ErrorReport("Персонаж не найден!").ShowDialog(this);
        }
    }
    private async void AddNewPhoto(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Title = "Выберите изображение персонажа",
            AllowMultiple = false,
            Filters = new List<FileDialogFilter>
        {
            new FileDialogFilter { Name = "Изображения", Extensions = { "jpg", "jpeg", "png", "bmp" } }
        }
        };

        var result = await dialog.ShowAsync(this); // this — текущее окно

        if (result != null && result.Length > 0)
        {
            selectedPhotoFilePath = result[0];
            photoFileName = Path.GetFileName(selectedPhotoFilePath);

            // (по желанию) отобразить выбранное изображение
            photo.Source = new Bitmap(selectedPhotoFilePath);
        }
        else
        {
            // Пользователь отменил выбор — установить фото по умолчанию
            selectedPhotoFilePath = null;
            photoFileName = "photoCheck.jpeg";
        }
    }
    private void OpenNewCharacter(int idThisChar)
    {
        idС = idThisChar;
        foreach (Character character in characterList)
        {
            if (character.Id == idС)
            {
                titleCharacter.Text = character.Name;
                persona.Text = character.Personality;
                hobby.Text = character.Hobbies;
                goal.Text = character.Goal;
                ens.Text = character.Ethnicity;
                fear.Text = character.Fears;
                minus.Text = character.Weaknesses;
                plus.Text = character.Strengths;
                study.Text = character.Occupation;
                descr.Text = character.Description;
                age.Text = character.Age.ToString();
                photo.Source = character.CoverBitmap;
                break;
            }
        }
        
    }

    private void ListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        if (allCharacter.SelectedItem != null)
        {
            var selectedCharacter = allCharacter.SelectedItem as Character;
            if (selectedCharacter != null)
            {
                charecterIdThisBook = selectedCharacter.Id; // Получаем id главы
                OpenNewCharacter(charecterIdThisBook);
            }
        }
    }
    private void CallBaza()
    {
        ListsStaticClass.listAllBooks.Clear();
        ListsStaticClass.listAllBooks = Baza.DbContext.Books.Select(book => new Book
        {
            Id = book.Id,
            Title = book.Title,
            Rating = book.Rating,
            Kolread = book.Kolread,
            Kolplan = book.Kolplan,
            Kolrev = book.Kolrev,
            AgeLimit = book.AgeLimit,
            PageCount = book.PageCount,
            CoverImage = book.CoverImage,
            Synopsis = book.Synopsis,
            Description = book.Description,
            IsAuthorBook = book.IsAuthorBook,
            Dateadd = book.Dateadd,
            SeriesId = book.SeriesId,
        }).ToList();

        ListsStaticClass.listAllCharacter.Clear();
        ListsStaticClass.listAllCharacter = Baza.DbContext.Characters.Select(charackter => new Character
        {
            Id = charackter.Id,
            BookId = charackter.BookId,
            Name = charackter.Name,
            ImageName = charackter.ImageName,
            Personality = charackter.Personality,
            Age = charackter.Age,
            GenderId = charackter.GenderId,
            Weaknesses = charackter.Weaknesses,
            Ethnicity = charackter.Ethnicity,
            Occupation = charackter.Occupation,
            Strengths = charackter.Strengths,
            Description = charackter.Description,
            Fears = charackter.Fears,
            Goal = charackter.Goal,
            Hobbies = charackter.Hobbies,
        }).ToList();
    }

    private void Button_Click_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new Friends().Show();
        Close();
    }

    private void Button_Click_2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        new CreatingBook(idThisBook).Show();
        Close();
    }
}