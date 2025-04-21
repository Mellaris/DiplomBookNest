using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace DiplomTwo
{
    public class EmailSender
    {
        public static void SendVerificationCode(string toEmail, string code)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Книжное приложение", "nastasia.bulaeva@ya.ru")); 
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = "Код подтверждения";

            message.Body = new TextPart("plain")
            {
                Text = $"Ваш код подтверждения: {code}"
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.yandex.com", 587, false); 
                client.Authenticate("nastasia.bulaeva@ya.ru", "wkdbhrxxgexdiszo");
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
