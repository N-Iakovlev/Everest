using Incoding.Core.CQRS.Core;
using Incoding.Core.Data;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everest.Domain;
public class AddCartItemsToOrderCommand : CommandBase
{
    public int UserId { get; set; }
    public string CreatorOrder { get; set; }
    public string Email { get; set; }
    public string Comment { get; set; }
    public Order.OfStatus Status { get; set; }
    public CartItem CartItems { get; set; }

    protected override void Execute()
    {
        var currentUser = Dispatcher.Query(new GetCurrentUserQuery()).Id;

        // Получаем корзину текущего пользователя
        var cartItems = Repository.Query<CartItem>()
                                  .Where(q => q.Cart.User.Id == currentUser)
                                  .ToList();

        // Создаем новый заказ
        var order = new Order()

        {   UserId = currentUser,
            CreatorOrder = CreatorOrder,
            Comment = Comment,
            Email = Email,
            Status = Order.OfStatus.New,
            OrderDate = DateTime.UtcNow,
        
        };

        // Сохраняем новый заказ в базе данных
        Repository.Save(order);

        // Переносим элементы корзины в заказ
        foreach (var cartItem in cartItems)
        {
            // Создаем новый элемент заказа и устанавливаем его свойства
            var orderItem = new OrderItem()
            {
                Order = order,
                Product = cartItem.Product
                
            };

            // Сохраняем элемент заказа в базе данных
            Repository.Save(orderItem);
        }

        // Очищаем корзину пользователя
        ClearUserCart(currentUser);
    }

    private void ClearUserCart(int userId)
    {
        // Находим корзину пользователя
        var cart = Repository.Query<Cart>().FirstOrDefault(q => q.User.Id == userId);

        if (cart != null)
        {
            // Удаляем все элементы корзины
            

            // Сохраняем изменения в базе данных
            Repository.SaveOrUpdate(cart);
        }
    }
}
public class ClearUserCartCommand : CommandBase
{
    public int UserId { get; set; }

    protected override void Execute()
    {
        ClearUserCart(UserId);
    }

    private void ClearUserCart(int userId)
    {
        // Находим корзину пользователя
        var cart = Repository.Query<Cart>().FirstOrDefault(q => q.User.Id == userId);

        if (cart != null)
        {
            // Удаляем все элементы корзины
            

            // Сохраняем изменения в базе данных
            Repository.SaveOrUpdate(cart);
        }
    }
}