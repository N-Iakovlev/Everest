using Incoding.Core.CQRS.Core;
namespace Everest.Domain;

public class GetContactsQuery : QueryBase<List<GetContactsQuery.Response>>
{
    protected override List<Response> ExecuteResult()
    {

        var query = Repository.Query<Contacts>();


        return query.Select(q => new Response
            {
                Id = q.Id,
                Adress = q.Adress,
                Phone = q.Phone,
                Email = q.Email,
                Company = q.Company,
                Domen = q.Domen

            })
            .ToList();
    }

    public class Response
    {
        public int? Id { get; set; }
        public string Adress { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Domen { get; set; }

    }
}