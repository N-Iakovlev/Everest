using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Incoding.Core.CQRS.Core;


namespace Everest.Domain;

public class GetCategoryQuery : QueryBase<List<GetCategoryQuery.Response>>
{

    protected override List<Response> ExecuteResult()
    {
        var query = Repository.Query<Category>();
        var typeMappings = new Dictionary<Category.OfType, string>
        {
            { Category.OfType.Product, "Товары" },
            { Category.OfType.Employee, "Сотрудники" }
        };

        return query.ToList().Select(q => new Response
            {
                Id = q.Id,
                Name = q.Name,
                Type = q.Type,
                TypeName = typeMappings[q.Type]
        })
            .ToList();
    }
    public class Response
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Category.OfType Type { get; set; }
        public string TypeName { get; set; }
    }
}

