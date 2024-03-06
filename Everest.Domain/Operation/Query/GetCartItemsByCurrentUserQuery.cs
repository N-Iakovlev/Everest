using Incoding.Core.CQRS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everest.Domain
{
    
    public class GetCartItemsByCurrentUserQuery : QueryBase<List<GetCartItemsByCurrentUserQuery.Response>>
    {
        
        public class Response
        {
            public string Product { get; set; }
            public decimal Price { get; set; }
            public int ProductId { get; set; }

            // Идентификатор элемента корзины
            public int Id { get; set; }

            // Идентификатор пользователя
            public int UserId { get; set; }
        }
        protected override List<Response> ExecuteResult()
        {
            var currentUser = Dispatcher.Query(new GetCurrentUserQuery()).Id;
            return Repository.Query<CartItem>()
                .Where(q => q.Cart.User.Id == currentUser) // Фильтруем элементы корзины по текущему пользователю
                                                           // Преобразуем элементы корзины в объекты типа Response
                .Select(q => new Response()
                {
                                                            // Имя продукта из элемента корзины
                    Product = q.Product.ProductName,
                                                            // Идентификатор продукта из элемента корзины
                    ProductId = q.Product.Id,
                                                            // Идентификатор элемента корзины
                    Id = q.Id,
                                                         // Идентификатор пользователя из элемента корзины
                    UserId = q.Cart.User.Id
                })
                                                        // Преобразуем результат в список и возвращаем
                .ToList();
        }
    }
}
