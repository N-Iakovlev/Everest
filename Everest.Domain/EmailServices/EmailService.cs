using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Everest.Domain

{
    public class EmailService
    {
        private readonly MailSettings _smtpSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(MailSettings smtpSettings, ILogger<EmailService> logger)
        {
            _smtpSettings = smtpSettings;
            _logger = logger;
        }

        public async Task SendEmailDefault(string to, string subject, string body)
        {
            try
            {
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

                _logger.LogInformation("Сообщение отправлено успешно");
            }
            catch (Exception e)
            {
                _logger.LogError(e.GetBaseException().Message);
            }
        }
    }
}