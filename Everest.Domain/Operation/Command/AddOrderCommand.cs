namespace Everest.Domain;

#region << Using >>

using FluentValidation;
using Incoding.Core.CQRS.Core;
using NHibernate.Criterion;

#endregion
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
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Comment { get; set; }
    private readonly EmailService _emailService;

    protected override void Execute()
    {
        var order = new Order();
        var currentUser = Dispatcher.Query(new GetCurrentUserQuery()).Id;
        // Получаем товары из корзины
        var cartItems = Repository.Query<CartItem>()
            .Where(q => q.Cart.User.Id == currentUser)
            .ToList();

        // Заполняем данные заказа
        order.Name = Name;
        order.Email = Email;
        order.Comment = Comment;
        order.Status = Order.OfStatus.New;
        order.OrderDate = DateTime.UtcNow;
        order.User = Repository.LoadById<User>(currentUser);

        // Сохраняем заказ
        Repository.SaveOrUpdate(order);

        // Добавляем товары в заказ
        foreach (var cartItem in cartItems)
        {
            var orderItem = new OrderItem
            {
                Order = order,
                Product = cartItem.Product,
            };
            Repository.Save(orderItem);
        }

        // Сохраняем изменения
        Repository.SaveOrUpdate(order);

        foreach (var cartItem in cartItems)
        {
            Repository.Delete(cartItem);
        }
        SendEmail(order);
    }

    private void SendEmail(Order order)
    {
        
            string subject = "Ваш заказ успешно создан";
            string body = $"Ваш заказ №{order.Id} успешно создан.";
            _emailService.SendEmailDefault(order.Email, subject, body).Wait();
        
       
    }

    public class Validator : AbstractValidator<AddOrderCommand>
    {
        public Validator()
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
                Name = order.Name,
                Email = order.Email,
                Comment = order.Comment,
            };
        }
    }

}
