using Microsoft.AspNetCore.Http;

namespace Everest.Domain;

#region << Using >>

using FluentValidation;
using Incoding.Core.CQRS.Core;

#endregion

public class DeleteEmailSettingsCommand : CommandBase
{
    public int Id { get; set; }

    protected override void Execute()
    {
        Repository.Delete(Repository.GetById<EmailSettings>(Id));
    }
}
public class AddOrEditEmailSettingsCommand : CommandBase
{
    public int? Id { get; set; }
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    protected override void Execute()
    {
        var isNew = Id.GetValueOrDefault() == 0;
        EmailSettings es = isNew ? new EmailSettings() : Repository.GetById<EmailSettings>(Id.GetValueOrDefault());


        es.SmtpServer = SmtpServer;
        es.Port = Port;
        es.Username = Username;
        es.Password = Password;
        

        Repository.SaveOrUpdate(es);
    }
    public class Validator : AbstractValidator<AddOrEditEmailSettingsCommand>
    {
        public Validator()
        {
            RuleFor(es => es.SmtpServer).NotEmpty();
            RuleFor(es => es.Username).NotEmpty();
            RuleFor(es => es.Port).NotEmpty();
            RuleFor(es => es.Password).NotEmpty();
        }
    } 
    public class AsQuery : QueryBase<AddOrEditEmailSettingsCommand>
    {
        public int Id { get; set; }

        protected override AddOrEditEmailSettingsCommand ExecuteResult()
        {
            var es = Repository.GetById<EmailSettings>(Id);
            if (es == null)
                return new AddOrEditEmailSettingsCommand();

            return new AddOrEditEmailSettingsCommand()
            {
                SmtpServer = es.SmtpServer,
                Port = es.Port,
                Username = es.Username,
                Password = es.Password
            };
        }
    }
}