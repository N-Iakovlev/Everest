using Everest.Domain;
using FluentValidation;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Everest.Domain
{
    public class AddOrderCommand : CommandBase
    {
        public int? Id { get; set; }
        public string NameOfOrder { get; set; }
        public string Email { get; set; }
        public DateTime OrderDate { get; set; }
        public string Comment { get; set; }
        public Order.OfStatus Status { get; set; }
        public int UserId { get; set; }
        public IList<OrderDetail> OrderDetails { get; set; }

        protected override void Execute()
        {
            var currentUser = Dispatcher.Query(new GetCurrentUserQuery()).Id;
            var isNew = Id == 0;
            Order order = isNew ? new Order() : Repository.GetById<Order>(Id);

            order.UserId = UserId;
            order.Comment = Comment;
            order.Email = Email;
            order.NameOfOrder = NameOfOrder;
            order.OrderDate = OrderDate;
            order.Status = Status;

            if (isNew)
            {
                order.Status = Order.OfStatus.New;
                order.OrderDate = DateTime.UtcNow;
                order.OrderDetails = Repository.Query<CartItem>()
                                                .Where(cartItem => cartItem.Cart.User.Id == currentUser)
                                                .Select(cartItem => new OrderDetail
                                                {
                                                    OrderId = order.Id,
                                                    ProductId = cartItem.Product.Id
                                                })
                                                .ToList();

                Repository.SaveOrUpdate(order);
            }
        }

        public class AddOrderCommandValidator : AbstractValidator<AddOrderCommand>
        {
            public AddOrderCommandValidator()
            {
                RuleFor(x => x.UserId).NotEmpty().WithMessage("User Id is required");
            }
        }

        public class AsQuery : QueryBase<AddOrderCommand>
        {
            public int Id { get; set; }

            protected override AddOrderCommand ExecuteResult()
            {
                var order = Repository.GetById<Order>(Id);
                if (order == null)
                    return new AddOrderCommand();

                return new AddOrderCommand()
                {
                    Id = order.Id,
                    Email = order.Email,
                    NameOfOrder = order.NameOfOrder,
                    OrderDetails = order.OrderDetails,
                    Comment = order.Comment,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    UserId = order.UserId,
                };
            }
        }
    }
}
