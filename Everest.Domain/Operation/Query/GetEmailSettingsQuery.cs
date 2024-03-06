#region << Using >>

using Incoding.Core.CQRS.Core;

#endregion


namespace Everest.Domain;

public class GetEmailSettingsQuery : QueryBase<List<GetEmailSettingsQuery.Response>>
{
    protected override List<Response> ExecuteResult()
    {

        var query = Repository.Query<EmailSettings>();

        return query.Select(q => new Response
            {
                Id = q.Id,
                SmtpServer = q.SmtpServer,
                Port = q.Port,
                Username = q.Username,
                Password = q.Password,

            })
            .ToList();

    }

    public class Response
    {
        public int Id { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }


}



