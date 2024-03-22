using Incoding.Core.CQRS.Core;
using System.Net;
using System.Net.Mail;

namespace Everest.Domain;

public class SendEmailCommand : CommandBase
{

    private readonly EmailService _emailService;

    public string To { get; set; }
    
    protected override void Execute()
    {
        void SendEmail(Order order)
        {

            string subject = "Ваш заказ успешно создан";
            string body = $"Ваш заказ №{order.Id} успешно создан.";
            _emailService.SendEmailDefault(order.Email, subject, body).Wait();
        }
    }

}


