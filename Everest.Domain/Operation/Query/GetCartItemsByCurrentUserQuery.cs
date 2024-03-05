using Incoding.Core.CQRS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everest.Domain;
public class GetCartItemsByCurrentUserQuery : QueryBase<List<GetCartItemsByCurrentUserQuery.Response>>
{
    public class Response
    {
        public string Product { get; set; }
        public decimal Price { get; set; }
        public int ProductId {get; set; }
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