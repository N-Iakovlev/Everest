using Incoding.Core.Block.IoC;
using Incoding.Core.CQRS.Core;
using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everest.Domain;
public class GetCurrentUserQuery : QueryBase<GetCurrentUserQuery.Response>
{
    public class Response
    {
        public int Id { get; set; }
    }

    protected override Response ExecuteResult()
    {
        var httpContext = IoCFactory.Instance.TryResolve<IHttpContextAccessor>().HttpContext;
        var cookieKey = "UserId";
        var tempId = httpContext.Request.Cookies[cookieKey];
        if (string.IsNullOrWhiteSpace(tempId))
        {
            tempId = Guid.NewGuid().ToString();
            httpContext.Response.Cookies.Append(cookieKey, tempId);
            Dispatcher.New().Push(new NewUserCommand() { TempId = tempId });
        }

        var userId = Repository.Query<User>().Single(q => q.TempId == tempId).Id;
        return new Response() { Id = userId };
    }
}

public class NewUserCommand : CommandBase
{
    protected override void Execute()
    {
        Repository.Save(new User() { TempId = TempId });
    }

    public string TempId { get; set; }
}
