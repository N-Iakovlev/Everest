using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluentValidation;
using Incoding.Core;
using Incoding.Core.CQRS.Core;
using NHibernate.Criterion;
using static Incoding.Core.Block.Scheduler.Persistence.DelayToScheduler.Where;
using static NHibernate.Engine.Query.CallableParser;

namespace Everest.Domain;

public class CloseCartCommand : CommandBase
{
    public string? Email { get; set; }
    public string Name { get; set; }
    public string Comment { get; set; }

    private readonly MailSettings _smtpSettings;
    protected override async void Execute()
    {
        
        var order = new Order();
        var currentUser = Dispatcher.Query(new GetCurrentUserQuery()).Id;
        var cartItems = Repository.Query<CartItem>()
            .Where(q => q.Cart.User.Id == currentUser)
            .ToList();
        order.User = Repository.LoadById<User>(currentUser);
        order.Email = Email;
        order.Comment = Comment;
        order.Name = Name;
        order.OrderDate = DateTime.Now;
        order.Status = Order.OfStatus.New;

        foreach (var cartItem in cartItems)
        {
            var orderItem = new OrderItem
            {
                Order = order,
                Product = cartItem.Product,
            };
            Repository.Save(orderItem);
        }

        Repository.Save(order);

        foreach (var cartItem in cartItems)
        {
            Repository.Delete(cartItem);
        }
        if (Email != null)
        {
            MailSender mailSender = new MailSender();

            string adminSubject = $"Новый заказ №{order.Id}";
            string adminBody = $"Получен новый заказ №{order.Id} от {Name}. Пожалуйста, проверьте панель администратора.";
            await mailSender.SendMailAsync("kentarmy@gmail.com", adminSubject, adminBody);

            string userSubject = $"Подтверждение заказа №{order.Id}";
            string userBody = $"Уважаемый(ая) {Name}, ваш заказ №{order.Id} был успешно оформлен.";
            await mailSender.SendMailAsync(Email, userSubject, userBody);
        }
    }
}


