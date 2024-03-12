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
    public string CreatorOrderName { get; set; }
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
        order.CreatorOrderName = CreatorOrderName;
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
        try
        {
            string subject = "Ваш заказ успешно создан";
            string body = $"Ваш заказ №{order.Id} успешно создан.";
            _emailService.SendEmailDefault(order.Email, subject, body).Wait();
        }
        catch (Exception ex)
        {
            // Обработка ошибок при отправке письма
            Console.WriteLine("Ошибка при отправке письма: " + ex.Message);
        }
    }

    public class Validator : AbstractValidator<AddOrderCommand>
    {
        public Validator()
        {
            RuleFor(order => order.Email).NotEmpty();
            RuleFor(order => order.CreatorOrderName).NotEmpty();
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
                CreatorOrderName = order.CreatorOrderName,
                Email = order.Email,
                Comment = order.Comment,
            };
        }
    }

}
