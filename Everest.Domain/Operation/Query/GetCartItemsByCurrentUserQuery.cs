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
    }


    protected override List<Response> ExecuteResult()
    {
        var currentUser = Dispatcher.Query(new GetCurrentUserQuery());
        return Repository.Query<CartItem>()
            .Where(q => q.Cart.Id == currentUser.Id)
            .Select(q => new Response()
            {
                Product = q.Product.ProductName,

            })
            .ToList();
    }
}