﻿namespace Everest.Domain;

#region << Using >>

using Incoding.Core.CQRS.Core;

#endregion

public class GetProductQuery : QueryBase<List<GetProductQuery.Response>>
{
    public string Search { get; set; }
    protected override List<Response> ExecuteResult()
    {
    
        var query = Repository.Query<Product>();

        if (!string.IsNullOrEmpty(Search))
        {
            query = query.Where(e => e.ProductName.Contains(Search));
        }

        return query.Select(q => new Response
            {
                Id = q.Id,
                ProductName = q.ProductName,
                Price = q.Price,
                LongDescription = q.LongDescription,
                ShortDescription = q.ShortDescription,
                
            })
            .ToList();
    
    }

    public class Response
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price {get; set; }
        public string LongDescription { get; set; }
        public string ShortDescription { get; set; }

    }

    
}