using Microsoft.AspNetCore.Http;

namespace Everest.Domain;

#region << Using >>

using FluentValidation;
using Incoding.Core.CQRS.Core;
using static Everest.Domain.Order;

#endregion
public class DeleteOrderCommand : CommandBase
{
    public int Id { get; set; }

    protected override void Execute()

    {
        Repository.Delete(Repository.GetById<Order>(Id));
    }
}

public class AddOrEditOrderCommand : CommandBase
{
    public int? Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Comment { get; set; }
    public IList<OrderItem> OrderItems { get; set; }
    public Order.OfStatus Status { get; set; }
    public User User { get; set; }


    protected override void Execute()
    {
        var isNew = Id.GetValueOrDefault() == 0;
        Order order = isNew ? new Order() : Repository.GetById<Order>(Id.GetValueOrDefault());

        // Если заказ новый, установить текущую дату и статус
        if (isNew)
        {
            order.OrderDate = DateTime.Now;
            order.Status = OfStatus.New;
        }

        // Заполнение полей заказа данными из команды
        order.Email = Email;
        order.Name = Name;
        order.Comment = Comment;
        order.OrderItems = OrderItems;
        order.Status = Status;
        order.User = User;
        Repository.SaveOrUpdate(order);
    }

    public class Validator : AbstractValidator<AddOrEditOrderCommand>
    {
        public Validator()
        {
            RuleFor(pr => pr.Name).NotEmpty();
        }
    }

    public class AsQuery : QueryBase<AddOrEditOrderCommand>
    {
        public int? Id { get; set; }

        protected override AddOrEditOrderCommand ExecuteResult()
        {
            var order = Repository.GetById<Order>(Id);
            if (order == null)
                return new AddOrEditOrderCommand();
            return new AddOrEditOrderCommand()
            {
                Id = order.Id,
                Name = order.Name,
                Email = order.Email,
                Comment = order.Comment,
                Status = order.Status,
                OrderItems = order.OrderItems
            };
        }

    }
}
