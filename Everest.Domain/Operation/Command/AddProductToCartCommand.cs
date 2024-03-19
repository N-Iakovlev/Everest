namespace Everest.Domain;
#region << Using >>

using Incoding.Core.CQRS.Core;
#endregion

public class AddProductToCartCommand : CommandBase
{
    public int ProductId { get; set; }
    protected override void Execute()
    {
        var currentUser = Dispatcher.Query(new GetCurrentUserQuery()).Id;
        var cart = Repository.Query<Cart>().FirstOrDefault(q => q.User.Id == currentUser)
                   ?? new Cart()
                   {
                       User = Repository.LoadById<User>(currentUser)
                   };
        Repository.SaveOrUpdate(cart);
        var cartItem = new CartItem()
        {
            Cart = cart,
            Product = Repository.GetById<Product>(ProductId)
        };
        Repository.Save(cartItem);
    }
}

public class DeleteCartItemCommand : CommandBase
{
    public int Id { get; set; }
    protected override void Execute()
    {
        Repository.Delete(Repository.GetById<CartItem>(Id));
    }
}
