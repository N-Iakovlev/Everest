using Incoding.Core.CQRS.Core;
using NHibernate.Criterion;
using System.Net;
using System.Net.Mail;

namespace Everest.Domain;

public class SendQuestionCommand : CommandBase
{
    private readonly MailSettings _smtpSettings;

    public string Contacts { get; set; }
    public string Name { get; set; }
    public string Question { get; set; }
    protected override async void Execute()
    {
        MailSender mailSender = new MailSender();
        string adminSubject = $"Новый заказ с сайта";
        string adminBody = $"Получен новый заказ свяжитесь с {Name}. контакты для связи : {Contacts}. Коментарий: {Question}";
        await mailSender.SendMailAsync("kentarmy@gmail.com", adminSubject, adminBody);
    }

}


