using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DiplomTwo.Models;
using System.Collections.Generic;
using System.Linq;

namespace DiplomTwo;

public partial class CodeCheckWindow : Window
{
    private List<VerificationCode> checkVerification = new List<VerificationCode>();
    private int idUserCheck;
    private Window loginWindow;
    public CodeCheckWindow()
    {
        InitializeComponent();
    }
    public CodeCheckWindow(int id, Window loginWindow)
    {
        InitializeComponent();
        CallBaza();
        idUserCheck = id;
        this.loginWindow = loginWindow;
    }
    private void CallBaza()
    {
        checkVerification.Clear();
        checkVerification = Baza.DbContext.VerificationCodes.Select(check => new VerificationCode
        {
            Id = check.Id,
            UserId = check.UserId,
            Code = check.Code,
        }).ToList();
    }
    private void TextBox_TextChanged(object? sender, Avalonia.Controls.TextChangedEventArgs e)
    {
        if(!string.IsNullOrEmpty(checkCode.Text))
        {
            foreach(VerificationCode code in checkVerification)
            {
                if(idUserCheck == code.UserId && checkCode.Text == code.Code)
                {
                    var verificationCodeToRemove = Baza.DbContext.VerificationCodes
                        .FirstOrDefault(vc => vc.UserId == idUserCheck && vc.Code == checkCode.Text); 
                    if(verificationCodeToRemove != null)
                    {
                        Baza.DbContext.VerificationCodes.Remove(verificationCodeToRemove);
                        Baza.DbContext.SaveChanges();
                    }

                    new personalAccount().Show();
                    Close();

                    loginWindow.Close();
                    break;
                }
            }
        }
    }
}