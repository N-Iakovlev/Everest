namespace Everest.Domain;

#region << Using >>

using Incoding.Core.CQRS.Core;

#endregion

public class GetProductQuery : QueryBase<List<GetProductQuery.Response>>
{
    protected override List<Response> ExecuteResult()
    {
        return Repository.Query<Product>()
                         .Select(q => new Response()
                                      {
                                              Id = q.Id,
                                              ProductName = q.ProductName,
                                              ProductCategory = q.ProductCategory
                                      })
                         .ToList();
    }

    public class Response
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }

    }
}