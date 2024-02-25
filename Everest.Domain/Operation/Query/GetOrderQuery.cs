using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Incoding.Core.CQRS.Core;
using Everest.Domain;

public class GetOrderQuery : QueryBase<List<GetOrderQuery.Response>>
{
    public string Search { get; set; }

    protected override List<Response> ExecuteResult()
    {
        var query = Repository.Query<Order>();
        return query.Select(q => new Response
            {
                Id = q.Id,
                Creator = q.Creator,
                CreateOrder = q.CreateDate,
                NunberOrder = q.NunberOrder,
                StatusOrder = q.StatusOrder,
                Notes = q.Notes,
                Phone = q.Phone,

                // Преобразуем список продуктов в строку для представления в классе Response
                Products = string.Join(", ", q.Products.Select(p => p.ProductName)),
                StatusChangeDate = q.StatusChangeDate
            })
            .ToList();
    }

    public class Response
    {
        public int Id { get; set; }
        public string Creator { get; set; }
        public DateTime CreateOrder { get; set; }
        public int NunberOrder { get; set; } 
        public string StatusOrder { get; set; }
        public string Notes { get; set; }
        public string Phone { get; set; }
        public string Products { get; set; } 
        public DateTime StatusChangeDate { get; set; }
    }
}