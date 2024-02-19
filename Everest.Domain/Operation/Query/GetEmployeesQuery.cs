namespace Everest.Domain;

#region << Using >>

using Incoding.Core.CQRS.Core;

#endregion

public class GetEmployeesQuery : QueryBase<List<GetEmployeesQuery.Response>>
{
    public string Search { get; set; }
    public int? CategoryId { get; set; }
    protected override List<Response> ExecuteResult()
    {
        var query = Repository.Query<Employee>();

        if (!string.IsNullOrEmpty(Search))
        {
            query = query.Where(e => e.FirstName.Contains(Search) || e.LastName.Contains(Search));
        }
        if (CategoryId.HasValue)
        {
            query = query.Where(e => e.EmployeeCategoryId == CategoryId);
        }

        return query.Select(q => new Response
            {
                Id = q.Id,
                FullName = $"{q.FirstName} {q.LastName}",
                Contacts = $"{q.Phone} {q.Email}",
                EmployeeCategoryId = q.EmployeeCategoryId


        })
            .ToList();
    }

    public class Response
    {
        public int Id { get; set; }

        public string FullName { get; set; }
        public string Contacts { get; set; }
        public int? EmployeeCategoryId { get; set; }

    }
    
}