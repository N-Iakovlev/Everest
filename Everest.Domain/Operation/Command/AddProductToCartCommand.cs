namespace Everest.Domain;
#region << Using >>

using Incoding.Core.CQRS.Core;
#endregion

public class AddProductToCartCommand : CommandBase
{
    public int ProductId { get; set; }
    

    protected override void Execute()
    {
        // Получаем идентификатор текущего пользователя
        var currentUser = Dispatcher.Query(new GetCurrentUserQuery()).Id;

        // Ищем корзину пользователя
        var cart = Repository.Query<Cart>().FirstOrDefault(q => q.User.Id == currentUser)
                   ?? new Cart()
                   {
                       User = Repository.LoadById<User>(currentUser)

                   };
        Repository.SaveOrUpdate(cart);
        // Создаем новый объект CartItem и добавляем его в корзину
        var cartItem = new CartItem()
        {
            Cart = cart,
            Product = Repository.GetById<Product>(ProductId) // Получаем продукт по его идентификатору
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
