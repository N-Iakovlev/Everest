using System.Net;
using System.Net.Mail;

namespace Everest.Domain;

public class MailSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
   
}

public class MailSender
{
    private readonly MailSettings _mailSettings = new()
    {
        Host = "smtp.gmail.com",
        Port = 587,
        UserName = "kentarmy@gmail.com",
        Password = "smmxhufuznjpwsoh"
    };

    public async Task SendMailAsync(string to, string subject, string body)
    {
        using (var message = new MailMessage())
        {
            message.From = new MailAddress(_mailSettings.UserName, "EverestTrade");
            message.To.Add(new MailAddress(to));
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtpClient = new SmtpClient(_mailSettings.Host, _mailSettings.Port))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(_mailSettings.UserName, _mailSettings.Password);
                smtpClient.EnableSsl = true;

                await smtpClient.SendMailAsync(message);
            }
        }
    }
}
//public void SendMail(string to, string subject, string body)
//     {
//         using (var message = new MailMessage())
//         {
//             message.From = new MailAddress(_mailSettings.UserName);
//             message.To.Add(new MailAddress(to));
//             message.Subject = subject;
//             message.Body = body;
//             message.IsBodyHtml = true; // если ваше письмо содержит HTML
//
//             using (var smtpClient = new SmtpClient(_mailSettings.Host, _mailSettings.Port))
//             {
//                 smtpClient.UseDefaultCredentials = false;
//                 smtpClient.Credentials = new NetworkCredential(_mailSettings.UserName, _mailSettings.Password);
//                 smtpClient.EnableSsl = true;
//
//                 smtpClient.Send(message);
//             }
//         }
//     }
// }