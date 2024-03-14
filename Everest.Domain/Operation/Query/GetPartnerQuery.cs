
using Incoding.Core.CQRS.Core;

namespace Everest.Domain;

public class GetPartnerQuery : QueryBase<List<GetPartnerQuery.Response>>

{
    protected override List<Response> ExecuteResult()
    {

        var query = Repository.Query<Partner>();

        return query.Select(q => new Response
            {
                Id = q.Id,
                CompanyName = q.CompanyName,
                Site = q.Site,
                Label = q.Label
        })
            .ToList();

    }

    public class Response
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Site { get; set; }
        public byte[] Label { get; set; }

    }

}