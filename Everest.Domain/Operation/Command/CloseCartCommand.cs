using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluentValidation;
using Incoding.Core;
using Incoding.Core.CQRS.Core;
using NHibernate.Criterion;
using static NHibernate.Engine.Query.CallableParser;

namespace Everest.Domain;

public class CloseCartCommand : CommandBase
{
   
    protected override void Execute()
    {
        var order = new Order();
        var currentUser = Dispatcher.Query(new GetCurrentUserQuery()).Id;
        var cartItems = Repository.Query<CartItem>()
            .Where(q => q.Cart.User.Id == currentUser)
            .ToList();
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
        Repository.Save(order);

        foreach (var cartItem in cartItems)
        {
            Repository.Delete(cartItem);
        }
    }
}
