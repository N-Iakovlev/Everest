namespace Everest.Domain;

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
            query = query.Where(e => e.ProductName.Contains(Search) || e.ProductArticl.Contains(Search) || e.Brand.Contains(Search));
        }

        return query.Select(q => new Response
            {
                Id = q.Id,
                ProductName = q.ProductName,
                Price = q.Price,
                Quantity = q.Quantity,
                ProductArticl = q.ProductArticl,
                Brand = q.Brand,
                Description = q.Description
            })
            .ToList();
    
    }

    public class Response
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price {get; set; }
        public int Quantity { get; set; }
        public string ProductArticl { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }

    }

    
}