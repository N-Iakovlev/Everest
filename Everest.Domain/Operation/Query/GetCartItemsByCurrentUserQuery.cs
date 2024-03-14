using Incoding.Core.CQRS.Core;

namespace Everest.Domain
{
    public class GetCartItemsByCurrentUserQuery : QueryBase<List<GetCartItemsByCurrentUserQuery.Response>>
    {
        public class Response
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public string Product { get; set; }
            public decimal Price { get; set; }
            public int ProductId { get; set; }
          
        }
        protected override List<Response> ExecuteResult()
        {
            var currentUser = Dispatcher.Query(new GetCurrentUserQuery()).Id;
            return Repository.Query<CartItem>()
                .Where(q => q.Cart.User.Id == currentUser) // Фильтруем элементы корзины по текущему пользователю
                                                           // Преобразуем элементы корзины в объекты типа Response
                .Select(q => new Response()
                {
                    Id = q.Id,
                    UserId = q.Cart.User.Id,
                    Product = q.Product.ProductName,
                    ProductId = q.Product.Id,
                    Price = q.Product.Price,
                })
                .ToList();
        }
    }
}
