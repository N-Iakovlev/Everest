using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everest.Domain;
using Incoding.Core.CQRS.Core;
namespace Everest.Domain;
public class GetContentQuery : QueryBase<List<GetContentQuery.Response>>
{
    
    public string Search { get; set; }
    protected override List<Response> ExecuteResult()
    {

        var query = Repository.Query<Content>();

        if (!string.IsNullOrEmpty(Search))
        {
            query = query.Where(e => e.ContentName.Contains(Search));
        }

        return query.Select(q => new Response
            {
                Id = q.Id,
                ContentName = q.ContentName,
            })
            .ToList();

    }

    public class Response
    {
        public int Id { get; set; }
        public string ContentName { get; set; }

    }
}

