using Everest.Domain;
using FluentValidation;
using Incoding.Core.CQRS.Core;
using Incoding.Core.Data;
using Microsoft.AspNetCore.Components;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Everest.Domain
{
    public class AddOrderCommand : CommandBase
    {
        public int? Id { get; set; }
        public string CreatorOrder { get; set; }
        public string Email { get; set; }
        public DateTime OrderDate { get; set; }
        public string Comment { get; set; }
        public Order.OfStatus Status { get; set; }
        public int UserId { get; set; }
        public IList<CartItem> CartItems { get; set; }

        protected override void Execute()
        {
            var isNew = Id.GetValueOrDefault() == 0;
            Order order = isNew ? new Order() : Repository.GetById<Order>(Id.GetValueOrDefault());
            var currentUser = Dispatcher.Query(new GetCurrentUserQuery());
            

            order.CreatorOrder = CreatorOrder;
            order.UserId = currentUser.Id;
            order.Status = Order.OfStatus.New;
            order.OrderDate = DateTime.UtcNow;
            order.Comment = Comment;
            order.Email = Email;

            Repository.SaveOrUpdate(order);
            Repository.Save(new OrderItem()
                {
                    Order = order,
                    Product = Repository.LoadById<Product>(CartItems),
                });
            // Добавляем блок для удаления CartItems

        }


        public class AddOrderCommandValidator : AbstractValidator<AddOrderCommand>
        {
            public AddOrderCommandValidator()
            {

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
                    CreatorOrder = order.CreatorOrder,
                    Comment = order.Comment,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    UserId = order.UserId,
                };
            }
        }
    }
}

