using Incoding.Core.CQRS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Everest.Domain.ShoppingCart;

namespace Everest.Domain;

public class GetShoppingCartQuery : QueryBase<List<GetShoppingCartQuery.Response>>
{

    protected override List<Response> ExecuteResult()
    {
        var query = Repository.Query<ShoppingCart>();
        return query.Select(q => new Response
            {
                Id = q.Id,
                UserId = q.UserId,
                CartLines = q.CartLines
        })
            .ToList();
    }
    public class Response
    {
        public int Id { get; set; }
        public  int UserId { get; set; }
        public List<CartLine> CartLines { get; set; }
    }

}

