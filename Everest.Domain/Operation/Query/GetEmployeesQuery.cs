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
            query = query.Where(e => e.FirstName.Contains(Search) || e.LastName.Contains(Search));
        }

        return query.Select(q => new Response
            {
                Id = q.Id,
                FullName = $"{q.FirstName} {q.LastName}",
            })
            .ToList();
    }

    public class Response
    {
        public int Id { get; set; }

        public string FullName { get; set; }
    }
    
}