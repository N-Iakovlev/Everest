using Everest.Domain;
using FluentValidation;
using Incoding.Core.Block.IoC;
using Incoding.Core.CQRS.Core;
using Microsoft.AspNetCore.Http;

namespace Everest.Domain
{
    public class AddProductToCartCommand : CommandBase
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }

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

            // Сохраняем или обновляем корзину
            Repository.SaveOrUpdate(cart);

            // Создаем новый объект CartItem и добавляем его в корзину
            Repository.Save(new CartItem()
            {
                Cart = cart,
                Product = Repository.GetById<Product>(ProductId) // Получаем продукт по его идентификатору
            });
        }
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
