using Incoding.Core.CQRS.Core;

namespace Everest.Domain
{
    public class GetCartQuery : QueryBase<List<GetCartQuery.Response>>
    {
        public class Response
        {
            public string Product { get; set; }
            public decimal Price { get; set; }
            public int ProductId { get; set; }
            public int Id { get; set; }
        }

        protected override List<Response> ExecuteResult()
        {
            var currentUser = Dispatcher.Query(new GetCurrentUserQuery()).Id;
            return Repository.Query<CartItem>()
                             .Where(q => q.Cart.User.Id == currentUser)
                             .Select(q => new Response()
                             {
                                 Product = q.Product.ProductName,
                                 Price = q.Product.Price,
                                 ProductId = q.Product.Id,
                                 Id = q.Id

                             })
                             .ToList();
        }
    }
}