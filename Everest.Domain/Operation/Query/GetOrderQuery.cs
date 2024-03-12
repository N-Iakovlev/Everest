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
            public string NameOfOrder { get; set; }
        }

        protected override List<Response> ExecuteResult()
        {
            var currentUser = Dispatcher.Query(new GetCurrentUserQuery());
            var statusMappings = new Dictionary<Order.OfStatus, string>
            {
                { Order.OfStatus.New, "Новый" },
                { Order.OfStatus.Processing, "В обработке" },
                { Order.OfStatus.Сanceled, "Отправлен" },
                { Order.OfStatus.Completed, "Завершен" },
            };

            return Repository.Query<Order>()
                .Where(q => q.User.Id == currentUser.Id)
                .Select(q => new Response()
                {
                    Id = q.Id,
                    Status = statusMappings[q.Status],
                    UserId = q.User.Id,
                    Comment = q.Comment,
                    Email = q.Email,

                })
                .ToList();
        }
    }
}