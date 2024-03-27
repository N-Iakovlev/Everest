using Incoding.Core.CQRS.Core;
using Incoding.Core.Data;
using Incoding.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everest.Domain;

public class GetProductForDDQuery : QueryBase<List<GetProductQuery.Response>>
{
    public int? CategoryId { get; set; }

    protected override List<GetProductQuery.Response> ExecuteResult()
    {
        IQueryable<Product> query = Repository.Query<Product>();

        // Фильтрация по категории, если задан CategoryId
        if (CategoryId.HasValue)
        {
            query = query.Where(e => e.Id == CategoryId.Value);
        }

        // Проекция результатов запроса на объекты GetProductQuery.Response
        List<GetProductQuery.Response> result = query
            .Select(q => new GetProductQuery.Response
            {
                Id = q.Id,
                ProductName = q.ProductName,
                Price = q.Price,
                LongDescription = q.LongDescription,
                ShortDescription = q.ShortDescription,
                ProductPhoto = q.ProductPhoto,
                CategoryName = q.Category.Name
            })
            .ToList();

        return result;
    }
}
