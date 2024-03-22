using Incoding.Core.CQRS.Core;

namespace Everest.Domain
{
    public class GetOrderQuery : QueryBase<List<GetOrderQuery.Response>>
    {
        public class Response
        {
            public int Id { get; set; }
            public string Status { get; set; }
           
            public string Comment { get; set; }
            public string Email { get; set; }
            public string Name { get; set; }
            public DateTime OrderDate { get; set; }
           
        }

        protected override List<Response> ExecuteResult()
        {
            var statusMappings = new Dictionary<Order.OfStatus, string>
            {
                { Order.OfStatus.New, "Новый" },
                { Order.OfStatus.Processing, "В обработке" },
                { Order.OfStatus.Canceled, "Отменен" },
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
                    OrderDate = q.OrderDate,
                })
                .ToList();
        }
    }
}