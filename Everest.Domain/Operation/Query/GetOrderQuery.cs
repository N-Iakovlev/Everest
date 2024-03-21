using Incoding.Core.CQRS.Core;

namespace Everest.Domain
{
    public class GetOrderQuery : QueryBase<List<GetOrderQuery.Response>>
    {
        public class Response
        {
            public int Id { get; set; }
            public string Status { get; set; }
            public int UserId { get; set; }
            public string Comment { get; set; }
            public string Email { get; set; }
            public IList<OrderItem> OrderItems { get; set; }
            public string Name { get; set; }
            public DateTime OrderDate { get; set; }
            public string ProductList { get; set; } // Строка для списка товаров
        }

        protected override List<Response> ExecuteResult()
        {
            var statusMappings = new Dictionary<Order.OfStatus, string>
            {
                { Order.OfStatus.New, "Новый" },
                { Order.OfStatus.Processing, "В обработке" },
                { Order.OfStatus.Сanceled, "Отменен" },
                { Order.OfStatus.Completed, "Завершен" },
            };
            return Repository.Query<Order>()
                .Select(q => new Response()
                {
                    Id = q.Id,
                    Name = q.Name,
                    Status =  statusMappings[q.Status],
                    Comment = q.Comment,
                    Email = q.Email,
                    OrderItems = q.OrderItems,
                    OrderDate = q.OrderDate,
                    // ProductList = string.Join(", ", q.OrderItems.Select(oi => oi.Product.ProductName))

                })
                .ToList();
        }
    }
}