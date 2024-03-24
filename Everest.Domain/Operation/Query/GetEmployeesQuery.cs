namespace Everest.Domain;

#region << Using >>
using Incoding.Core.CQRS.Core;
#endregion

public class GetEmployeesQuery : QueryBase<List<GetEmployeesQuery.Response>>
{
    public string Search { get; set; }
    protected override List<Response> ExecuteResult()
    {
        var query = Repository.Query<Employee>();
        if (!string.IsNullOrEmpty(Search))
        {
            string searchLower = Search.ToLower(); // Приводим строку поиска к нижнему регистру

            query = query.Where(e => e.FirstName.ToLower().Contains(searchLower) ||
                                     e.LastName.ToLower().Contains(searchLower));
        }

        return query.Select(q => new Response
                    {
                            Id = q.Id,
                            FullName = $"{q.FirstName} {q.LastName}",
                            Contacts = $"{q.Phone} {q.Email}",
                            Email = q.Email,
                            Phone = q.Phone
                    })
                    .ToList();
    }

    public class Response
    {
        public int Id { get; set; }

        public string FullName { get; set; }
        public string Contacts { get; set; }
        public string Phone {get; set; }
        public string Email { get; set; }
    }
    
}
