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
            query = query.Where(e => e.ShortDescription.Contains(Search));
        }

        return query.Select(q => new Response
                    {
                            Id = q.Id,
                            ShortDescription = q.ShortDescription,
                            LongDescription =q.LongDescription,
                    })
                    .ToList();
    }

    public class Response
    {
        public int Id { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }

    }
}