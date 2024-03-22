using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Everest.Domain;


public class EmailService
{
    private readonly MailSettings _smtpSettings;
    public EmailService(MailSettings smtpSettings)
    {
        _smtpSettings = smtpSettings ?? throw new ArgumentNullException(nameof(smtpSettings));
    }


    public async Task SendEmailDefault(string to, string subject, string body)
    {
        if (_smtpSettings == null)
        {
            throw new NullReferenceException("_smtpSettings is not initialized");
        }
        MailMessage mm = new MailMessage();
       mm.To.Add(to);
       mm.Subject = subject;
       mm.Body = body;
       mm.IsBodyHtml = true;
       mm.From = new MailAddress(_smtpSettings.UserName, "EverestTrade");

       using (SmtpClient smtp = new SmtpClient(_smtpSettings.Host))
       {
           smtp.Port = _smtpSettings.Port;
           smtp.UseDefaultCredentials = false;
           smtp.EnableSsl = true;
           smtp.Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password);

          await smtp.SendMailAsync(mm);
          
       }
        
        
    }
}
