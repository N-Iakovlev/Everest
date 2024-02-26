using Incoding.Core.CQRS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everest.Domain;

 public class GetCartLineQuery : QueryBase<List<GetCartLineQuery.Response>>
 {
     protected override List<Response> ExecuteResult()
     {
            var query = Repository.Query<CartLine>();
            return query.Select(q => new Response
                {
                    Id =q.Id,
                    Product = q.Product,
                    Quantity = q.Quantity,
                })
                .ToList();
     }
     public class Response
     { 
        public int Id { get; set; }
        public  Product Product { get; set; }
        public int Quantity { get; set; }
     
    }

 }

