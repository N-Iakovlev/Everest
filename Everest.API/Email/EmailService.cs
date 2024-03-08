using System.Net;
using System.Net.Mail;

namespace Everest.API;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string body);
}

public class EmailService : IEmailService
{
    private readonly SmtpClient _smtpClient;

    public EmailService(string smtpServer, int port, string fromEmail, string password)
    {
        _smtpClient = new SmtpClient(smtpServer, port)
        {
            Credentials = new NetworkCredential(fromEmail, password),
            EnableSsl = true
        };
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(((NetworkCredential)_smtpClient.Credentials).UserName),

                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            await _smtpClient.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            // Обработка ошибок при отправке письма
            Console.WriteLine($"Ошибка при отправке письма: {ex.Message}");
            throw; // Можно выбросить исключение для дальнейшей обработки на верхнем уровне
        }
        finally
        {
            _smtpClient.Dispose(); // Освобождаем ресурсы
        }
    }
}

