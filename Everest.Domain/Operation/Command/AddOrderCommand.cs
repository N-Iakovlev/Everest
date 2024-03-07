﻿using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Everest.Domain;

#region << Using >>

using FluentValidation;
using Incoding.Core.CQRS.Core;
using NHibernate.Engine;

#endregion

public class AddOrderCommand : CommandBase
{
    public string CreatorOrder { get; set; }
    public string Email { get; set; }
    public string Comment { get; set; }

    protected override void Execute()
    {
        var currentUser = Dispatcher.Query(new GetCurrentUserQuery()).Id;
        
        var order = Repository.Query<Order>().FirstOrDefault(order => order.User.Id == currentUser);
        var cartItems = Repository.Query<CartItem>()
            .Where(q => q.Cart.User.Id == currentUser)
            .ToList();


        order = new Order();


            order.CreatorOrder = CreatorOrder;
            order.Email = Email;
            order.Comment = Comment;
            order.Status = Order.OfStatus.New;
            order.OrderDate = DateTime.UtcNow;
            order.User = Repository.LoadById<User>(currentUser);
            

            foreach (var cartItem in cartItems)
            {
                var orderItem = new OrderItem
                {
                    Order = order,
                    Product = cartItem.Product,
                };
                Repository.Save(orderItem);
            }

            // Очистка корзины после оформления заказа
            foreach (var cartItem in cartItems)
            {
                Repository.Delete(cartItem);
            }

            
       
           // order.CreatorOrder = CreatorOrder;
           // order.Email = Email;
           // order.Comment = Comment;
            Repository.SaveOrUpdate(order);
    }
}