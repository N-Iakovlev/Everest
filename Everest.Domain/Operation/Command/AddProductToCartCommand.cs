using FluentValidation;
using Incoding.Core.Block.IoC;
using Incoding.Core.CQRS.Core;
using Microsoft.AspNetCore.Http;

namespace Everest.Domain;

public class DeleteCartCommand : CommandBase
{
    public int Id { get; set; }

    protected override void Execute()
    {
        Repository.Delete(Repository.GetById<CartItem>(Id));
    }
}


public class AddProductToCartCommand : CommandBase
{
    public  int ProductId { get; set; }
    
    protected override void Execute()
    {
        var currentUser = Dispatcher.Query(new GetCurrentUserQuery());
        var cart = Repository.Query<Cart>().FirstOrDefault(q => q.User.Id == currentUser.Id) ?? new Cart()
        {
            User = Repository.LoadById<User>(currentUser.Id)
        };
       
        Repository.SaveOrUpdate(cart);

        Repository.Save(new CartItem()
        {
            Cart = cart,
            Product = Repository.LoadById<Product>(ProductId),
        });
        
    }
}
public class DeleteCartItemCommand :CommandBase
{
    public int Id { get; set; }
    protected override void Execute()
    {
        Repository.Delete(Repository.GetById<CartItem>(Id));
    }
}
