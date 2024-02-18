namespace Everest.Domain;

#region << Using >>

using Incoding.Core.CQRS.Core;

#endregion

public class GetEmployeesQuery : QueryBase<List<GetEmployeesQuery.Response>>
{
    protected override List<Response> ExecuteResult()
    {
        return Repository.Query<Employee>()
                         .Select(q => new Response()
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