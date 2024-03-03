using Incoding.Core.CQRS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everest.Domain;

public class GetOrderQuery : QueryBase<List<GetOrderQuery.Response>>
{
    public class Response
    {
        public int Id { get; set; }
        public string Status { get; set; } // Изменил тип на string, чтобы хранить строковое представление статуса
        public string? NoteFromEmploee { get; set; }
        public IList<OrderDetail> Details { get; set; }
        
        

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
            .Where(q => q.UserId == currentUser.Id) //  проверку UserId
            .Select(q => new Response()
            {
                Id = q.Id,
                Status = statusMappings[q.Status], // Получаем строковое представление статуса из словаря
               
                Details = q.OrderDetails // Предполагается, что у заказа есть свойство OrderDetails, содержащее список деталей заказа
            })
            .ToList();
    }
}
