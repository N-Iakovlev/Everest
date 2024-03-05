using Incoding.Core.CQRS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            public IList<OrderDetail> OrderDetails { get; set; }
            public string NameOfOrder { get; set; }
        }

        protected override List<Response> ExecuteResult()
        {
            var currentUser = Dispatcher.Query(new GetCurrentUserQuery());
            var statusMappings = new Dictionary<Order.OfStatus, string>
            {
                { Order.OfStatus.New, "Новый" },
                { Order.OfStatus.Processing, "В обработке" },
                { Order.OfStatus.Shipped, "Отправлен" },
                { Order.OfStatus.Completed, "Завершен" },
            };

            return Repository.Query<Order>()
                .Where(q => q.UserId == currentUser.Id)
                .Select(q => new Response()
                {
                    Id = q.Id,
                    Status = statusMappings[q.Status],
                    UserId = q.UserId,
                    Comment = q.Comment,
                    Email = q.Email,
                    OrderDetails = q.OrderDetails,
                    NameOfOrder = q.NameOfOrder
                })
                .ToList();
        }
    }
}