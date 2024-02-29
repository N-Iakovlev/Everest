using Incoding.Core.CQRS.Core;
namespace Everest.Domain;

public class GetCartQuery : QueryBase<List<GetCartQuery.Response>>
{
    protected override List<Response> ExecuteResult()
    {
        var query = Repository.Query<Cart>();
        return query.Select(q => new Response
            {
                Id = q.Id,
                UserId = q.User.Id,
                
                
                
            })
            .ToList();
    }

    public class Response
    {
        public int Id { get; set; }
        public int UserId { get; set; }
    }
}
