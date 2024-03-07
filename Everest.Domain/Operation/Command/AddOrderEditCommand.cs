using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Incoding.Core.CQRS.Core;

namespace Everest.Domain;
///public class AddEditOrderCommand : CommandBase
//{
 //  public string CreatorOrder { get; set; }
 //  public string Email { get; set; }
 //  public string Comment { get; set; }
 //  public Order.OfStatus Status { get; set; }
 //  public int UserId { get; set; }
 //
 //  protected override void Execute()
 //  {
 //      var currentUser = Dispatcher.Query(new GetCurrentUserQuery()).Id;
 //
 //      // Создаем новый заказ на основе полученных данных
 //      var order = new Order
 //      {
 //          CreatorOrder = CreatorOrder,
 //          Email = Email,
 //          Comment = Comment,
 //          Status = Status,
 //          OrderDate = DateTime.UtcNow,
 //          User = Repository.LoadById<User>(currentUser)
 //      };
 //
 //      // Сохраняем заказ в базе данных
 //      Repository.Save(order);
 //  }
 //
 //
 // Валидатор для команды AddOrderCommand
 //blic class AddEditOrderCommandValidator : AbstractValidator<AddEditOrderCommand>
 //
 //  public AddEditOrderCommandValidator()
 //  {
 //      // Здесь можно добавить правила валидации для полей команды
 //      RuleFor(command => command.Email).NotEmpty().EmailAddress();
 //      RuleFor(command => command.UserId).NotEmpty();
 //      // Добавьте другие правила валидации по необходимости
 //  }
 //
 //
 //public class AddEditOrderCommand : CommandBase
 //{
 //    public string CreatorOrder { get; set; }
 //    public string Email { get; set; }
 //    public string Comment { get; set; }
 //
 //    protected override void Execute()
 //    {
 //        var currentUser = Dispatcher.Query(new GetCurrentUserQuery()).Id;
 //        var currentCart = Repository.Query<Cart>().FirstOrDefault(cart => cart.User.Id == currentUser);
 //
 //        if (currentCart == null || !currentCart.CartItems.Any())
 //        {
 //            throw new InvalidOperationException("There are no products in the user's cart to create an order.");
 //        }
 //
 //        // Получаем существующий заказ для текущего пользователя, если он есть
 //        var existingOrder = Repository.Query<Order>().FirstOrDefault(order => order.User.Id == currentUser);
 //
 //        if (existingOrder == null)
 //        {
 //            // Создаем новый заказ, так как его еще нет
 //            var order = new Order
 //            {
 //                CreatorOrder = CreatorOrder,
 //                Email = Email,
 //                Comment = Comment,
 //                Status = Order.OfStatus.New,
 //                OrderDate = DateTime.UtcNow,
 //                User = Repository.GetById<User>(currentUser)
 //            };
 //
 //            // Добавляем элементы корзины в заказ
 //            foreach (var cartItem in currentCart.CartItems)
 //            {
 //                var orderItem = new OrderItem
 //                {
 //                    Order = order,
 //                    Product = cartItem.Product
 //                };
 //                Repository.Save(orderItem);
 //            }
 //
 //            // Сохраняем новый заказ в базе данных
 //            Repository.Save(order);
 //        }
 //        else
 //        {
 //            // Обновляем существующий заказ и добавляем в него новые элементы корзины
 //            existingOrder.CreatorOrder = CreatorOrder;
 //            existingOrder.Email = Email;
 //            existingOrder.Comment = Comment;
 //
 //            // Добавляем новые элементы корзины в заказ
 //            foreach (var cartItem in currentCart.CartItems)
 //            {
 //                // Проверяем, есть ли уже такой элемент в заказе
 //                var existingOrderItem = existingOrder.OrderItems.FirstOrDefault(oi => oi.Product.Id == cartItem.Product.Id);
 //
 //                if (existingOrderItem == null)
 //                {
 //                    // Если элемент еще не существует в заказе, то добавляем его
 //                    var orderItem = new OrderItem
 //                    {
 //                        Order = existingOrder,
 //                        Product = cartItem.Product
 //                    };
 //                    Repository.Save(orderItem);
 //                }
 //                else
 //                {
 //                    // Иначе можно обновить количество товара в существующем элементе заказа
 //                    // Например: existingOrderItem.Quantity += cartItem.Quantity;
 //                }
 //            }
 //
 //            // Сохраняем обновленный заказ в базе данных
 //            Repository.SaveOrUpdate(existingOrder);
 //        }
 //    }
 //}
