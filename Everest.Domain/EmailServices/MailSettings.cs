using System.Net.Mail;
using System.Net;

namespace Everest.Domain;
public class MailSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}

