using Incoding.Core.CQRS.Core;

namespace Everest.Domain;

public class GetOrderItemsQuery : QueryBase<List<GetOrderItemsQuery.Response>>
{
    public class Response
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }

    public int OrderId { get; set; }

    protected override List<Response> ExecuteResult()
    {
        var orderItems = Repository.Query<OrderItem>()
            .Where(oi => oi.Order.Id == OrderId)
            .ToList();

        return orderItems.Select(item => new Response
        {
            OrderId = item.Order.Id,
            ProductId = item.Product.Id,
            ProductName = item.Product.ProductName,
            Price = item.Product.Price
        }).ToList();
    }
}
