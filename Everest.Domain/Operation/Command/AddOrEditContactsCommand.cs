namespace Everest.Domain;

#region << Using >>
using FluentValidation;
using HandlebarsDotNet;
using Incoding.Core.CQRS.Core;
using Microsoft.AspNetCore.Http;
#endregion

public class DeleteContactsCommand : CommandBase
{
    public int Id { get; set; }

    protected override void Execute()
    {
        Repository.Delete(Repository.GetById<Contacts>(Id));
    }
}

public class AddOrEditContactsCommand : CommandBase
{
    public int? Id { get; set; }
    public string Adress { get; set; }
    public int Phone { get; set; }
    public string Email { get; set; }
    public string Company { get; set; }
    public string Domen { get; set; }

    protected override void Execute()
    {
        var isNew = Id.GetValueOrDefault() == 0;
        Contacts contact = isNew ? new Contacts() : Repository.GetById<Contacts>(Id.GetValueOrDefault());
        contact.Adress = Adress;
        contact.Phone = Phone;
        contact.Email = Email;
        contact.Company = Company;
        contact.Domen = Domen;


        Repository.SaveOrUpdate(contact);
    }

    public class Validator : AbstractValidator<AddOrEditContactsCommand>
    {
        public Validator()
        {
        }
    }

    public class AsQuery : QueryBase<AddOrEditContactsCommand>
    {
        public int Id { get; set; }

        protected override AddOrEditContactsCommand ExecuteResult()
        {
            var contact = Repository.GetById<Contacts>(Id);
            if (contact == null)
                return new AddOrEditContactsCommand();

            return new AddOrEditContactsCommand()
            {
                Id = contact.Id,
                Adress = contact.Adress,
                Phone = contact.Phone,
                Email = contact.Email,
                Company = contact.Company,
                Domen = contact.Domen
            };
        }
    }
}