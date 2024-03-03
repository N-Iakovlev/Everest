using Incoding.Core.CQRS.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Everest.Domain;

public class DeleteOrderCommand : CommandBase
{
    public int Id { get; set; }

    protected override void Execute()
    {
        Repository.Delete(Repository.GetById<Order>(Id));
    }
}

public class AddOrderCommand : CommandBase
{
    public int? Id { get; set; }
    public List<CartItem> CartItems { get; set; }

    protected override void Execute()
    {
        var currentUser = Dispatcher.Query(new GetCurrentUserQuery());
        Order order;

        if (Id.HasValue) // Если указан идентификатор заказа, обновляем существующий заказ
        {
            order = Repository.GetById<Order>(Id.Value);

            // Проверяем, принадлежит ли заказ текущему пользователю, чтобы избежать изменения заказов других пользователей
            if (order.UserId != currentUser.Id)
            {
                throw new UnauthorizedAccessException("You are not authorized to edit this order.");
            }

            // Очищаем существующие детали заказа перед добавлением новых
            order.OrderDetails.Clear();
        }
        else // Иначе создаем новый заказ
        {
            order = new Order
            {
                Status = Order.OfStatus.New,
                OrderDate = DateTime.Now,
                UserId = currentUser.Id
            };
        }

        // Добавляем элементы корзины в заказ
        foreach (var cartItem in CartItems)
        {
            var orderDetail = new OrderDetail
            {
                CartId = cartItem.Cart.Id,
                ProductId = cartItem.Product.Id
            };
            order.OrderDetails.Add(orderDetail);
        }

        Repository.Save(order);
        
    }
}

