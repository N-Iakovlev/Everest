using Incoding.Core.CQRS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Engine;

namespace Everest.Domain;
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
        public int UserId { get; set; }
        public List<CartItem> CartItems { get; set; }

        protected override void Execute()
        {
            var currentUser = Dispatcher.Query(new GetCurrentUserQuery());
        // Создание нового заказа в базе данных
        var order = new Order
            {
                User = UserId,
                OrderDate = DateTime.Now,
                Status = Status
            };

            // Добавление товаров из корзины в заказ
            foreach (var cartItem in CartItems)
            {
                var orderItem = new OrderItem
                {
                    Order = order,
                    ProductId = cartItem.ProductId,
                    Price = cartItem.Product.Price // Предполагается, что цена продукта уже есть в объекте CartItem.Product
                };
                order.OrderItems.Add(orderItem);
            }

            // Сохранение нового заказа в базе данных
            Repository.Save(order);
    }

        protected override void Execute()
        {
            throw new NotImplementedException();
        }
    }
